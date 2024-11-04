
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Sotex.Api.Interfaces;
using Sotex.Api.Model;
using Newtonsoft.Json;
using System.Text;

using Sotex.Api.Services.DependencyInjection;
using Sotex.Api.Dto.MenuDto;
using Sotex.Api.Repo;
using AutoMapper;
using System.Globalization;
using Sotex.Api.Dto.JsonResponseDto;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;

namespace Sotex.Api.Services
{
    public class MenuService : IMenuService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private ResizeImage _resizeImage;
        private readonly IMapper _mapper;
        private readonly MenuRepo _menuRepo;
        public MenuService(HttpClient httpClient, IOptions<OpenAISettings> options, ResizeImage resizeImage, IMapper mapper, MenuRepo menuRepo)
        {
            _httpClient = httpClient;
            _apiKey = options.Value.ApiKey;
            _resizeImage = resizeImage;
            _mapper = mapper;
            _menuRepo = menuRepo;
        }

        public async Task<Menu> ParseAndSaveMenuFromFileAsync(IFormFile file, string purpose, Guid userId)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File cannot be null or empty.");
            }

            if(await _menuRepo.HasActiveOrUpcomingMenuAsync(userId))
            {
                throw new InvalidOperationException("You cannot upload a new menu because an active or scheduled menu already exists.");
            }


            string base64Image;
            using (var fileStream = file.OpenReadStream())
            {
                base64Image = _resizeImage.Resize(fileStream, maxWidth: 800, maxHeight: 800);
            }

            var requestContent = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
            new
            {
                role = "user",
                content = new object[]
                {
                    new
                    {
                        type = "text",
                        text = $"{purpose}: Please provide the content of the image as JSON in the following structure: " +
                        "{" +
                        "  \"day\": \"string\"," +
                        "  \"dishes\": [" +
                        "    {" +
                        "      \"name\": \"string\"," +
                        "      \"price\": number" +
                        "    }" +
                        "  ]," +
                        "  \"side_dishes\": [\"string\"]," +
                        "  \"special_offer\": \"string\"," +
                        "  \"order_info\": {" +
                        "    \"phone\": \"string\"," +
                        "    \"note\": \"string\"" +
                        "  }" +
                        "}"
                    },
                    new { type = "image_url", image_url = new { url = $"data:image/jpeg;base64,{base64Image}" } }
                }
            }
        },
                max_tokens = 300
            };

            var jsonContent = JsonConvert.SerializeObject(requestContent);
            Console.WriteLine("Request Payload: " + jsonContent);  // Log the payload

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
            {
                Content = httpContent
            };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<ResponseDto>(responseContent);

                var content = jsonResponse.choices[0].message.content.ToString().Trim();

                var jsonRegex = new Regex(@"\{.*\}", RegexOptions.Singleline);
                var match = jsonRegex.Match(content);

                if (match.Success)
                {
                    string jsonPart = match.Value;
                    Console.WriteLine("Extracted JSON: " + jsonPart);  // Log extracted JSON

                    var menuData = JsonConvert.DeserializeObject<AddMenuDto>(jsonPart);

                    if (menuData == null)
                    {
                        throw new Exception("Failed to deserialize menu data or menu is null.");
                    }

                    var menuDto = new Menu
                    {
                        Name = menuData.Day,
                        Dishes = menuData.Dishes.Select(dish => new Dish
                        {
                            Name = dish.Name,
                            Price = decimal.Parse(dish.Price.ToString(), CultureInfo.InvariantCulture)
                        }).ToList(),
                        SideDishes = menuData.Sides.Select(side => new SideDish { Name = side }).ToList(),
                        SpecialOffer = menuData.SpecialOffer,
                        OrderInfo = new OrderInfo
                        {
                            Phone = menuData.OrderInfo.Phone,
                            Note = menuData.OrderInfo.Note,
                        },
                        UserId = userId 
                    };

                    var now = DateTime.Now;
                    var startDate = now.AddDays(1).Date;
                    var endDate = startDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                    menuDto.StartDate = TimeZoneInfo.ConvertTimeToUtc(startDate);
                    menuDto.EndDate = TimeZoneInfo.ConvertTimeToUtc(endDate);

                    menuDto.IsActive = false;
                    menuDto.IsActiveTomorrow = startDate == now.AddDays(1).Date;

                    try
                    {
                        var savedMenu = await _menuRepo.AddMenuAsync(menuDto);
                        return savedMenu;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException?.Message);
                        throw;
                    }
                }
                else
                {
                    throw new Exception("No JSON content found in the response.");
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error calling OpenAI API: {response.StatusCode} - {errorContent}");
            }
        }

        public async Task<MenuDto> ListMenuItemsAsync()
        {
            var menus = await _menuRepo.GetAllMenusAsync();

            if (menus == null || !menus.Any())
            {
                throw new InvalidOperationException("No menu found for this user");
            }

            var activeMenu = menus.FirstOrDefault(menu => menu.IsActive || menu.IsActiveTomorrow);

            if(activeMenu == null)
            {
                throw new InvalidOperationException("No active or scheduled menu found. Please upload a new menu.");
            }

            Console.WriteLine($"Menu Dishes Count: {activeMenu.Dishes.Count}");
            Console.WriteLine($"Menu SideDishes Count: {activeMenu.SideDishes.Count}");

            var menuDto = new MenuDto();

            foreach (var dish in activeMenu.Dishes)
            {
                menuDto.Dishes.Add(new DishDto
                {
                    DishId = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price.ToString()
                });
            }

            foreach (var sideDish in activeMenu.SideDishes)
            {
                menuDto.SideDishes.Add(new SideDishDto
                {
                    SideDishId = sideDish.Id,
                    Name = sideDish.Name,
                });
            }
            return menuDto;
        }

        public async Task<(bool IsActive, bool IsActiveTomorrow)> GetMenuStatusAsync()
        {
            var menus = await _menuRepo.GetAllMenusAsync();

            bool isActive = false;
            bool isActiveTomorrow = false;

            foreach(var menu in menus)
            {
                if(menu.IsActive)
                {
                    isActive = true;
                }

                if(menu.IsActiveTomorrow)
                {
                    isActiveTomorrow = true;
                }

                if(isActive && isActiveTomorrow)
                {
                    break;
                }
            }

            //if(!isActive && !isActiveTomorrow)
            //{
            //    throw new InvalidOperationException("No active menu found. Please upload a new menu.");
            //}

            return (isActive, isActiveTomorrow);
        }
    }
}
