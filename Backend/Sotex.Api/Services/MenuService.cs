
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

        //public async Task<string> ParseImageFromFileAsync(IFormFile file, string purpose)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        throw new ArgumentException("File cannot be null or empty.");
        //    }


        //    string base64Image;
        //    using (var fileStream = file.OpenReadStream())
        //    {
        //        base64Image = _resizeImage.Resize(fileStream, maxWidth: 800, maxHeight: 800);
        //    }

        //    var requestContent = new
        //    {
        //        model = "gpt-4o-mini",
        //        messages = new[]
        //        {
        //            new
        //            {
        //                role = "user",
        //                content = new object[]
        //                {
        //                    new { type = "text", text = $"{purpose}: Can you describe the content of the image as JSON?" },
        //                    new
        //                    {
        //                        type = "image_url",
        //                        image_url = new
        //                        {
        //                            url = $"data:image/jpeg;base64,{base64Image}" 
        //                        }
        //                    }
        //                }
        //            }
        //        },
        //        max_tokens = 300
        //    };

        //    var jsonContent = JsonConvert.SerializeObject(requestContent);
        //    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //    var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
        //    {
        //        Content = httpContent
        //    };
        //    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        //    var response = await _httpClient.SendAsync(requestMessage);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var responseContent = await response.Content.ReadAsStringAsync();

        //        dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);
        //        string content = jsonResponse.choices[0].message.content;

        //        var match = System.Text.RegularExpressions.Regex.Match(content, @"```json(.*?)```", System.Text.RegularExpressions.RegexOptions.Singleline);
        //        if (match.Success)
        //        {
        //            string extractedJson = match.Groups[1].Value.Trim();
        //            return extractedJson; 
        //        }

        //        return content;
        //    }
        //    else
        //    {
        //        throw new Exception($"Error calling OpenAI API: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
        //    }
        //}

        public async Task<Menu> ParseAndSaveMenuFromFileAsync(IFormFile file, string purpose)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File cannot be null or empty.");
            }

            string base64Image;
            using (var fileStream = file.OpenReadStream())
            {
                base64Image = _resizeImage.Resize(fileStream, maxWidth: 800, maxHeight: 800);
            }

            // Request to OpenAI API
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
                    new { type = "text", text = $"{purpose}: Can you describe the content of the image as JSON?" },
                    new { type = "image_url", image_url = new { url = $"data:image/jpeg;base64,{base64Image}" } }
                }
            }
        },
                max_tokens = 300
            };

            var jsonContent = JsonConvert.SerializeObject(requestContent);
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

                // Extract the JSON content from the response
                var content = jsonResponse.choices[0].message.content.ToString().Trim('`');

                // Deserialize the content into AddMenuDto
                var menuData = JsonConvert.DeserializeObject<AddMenuDto>(content);

                // Map AddMenuDto to your database models
                var menuDto = new Menu
                {
                    Name = menuData.Day,  // Set menu name as "day"
                    Dishes = menuData.Menu.Dishes.Select(dish => new Dish
                    {
                        Name = dish.Name,
                        Price = decimal.Parse(dish.Price.Replace(",", "."), CultureInfo.InvariantCulture)  // Convert to decimal
                    }).ToList(),
                    SideDishes = menuData.Menu.Sides.Select(side => new SideDish { Name = side }).ToList()
                };

                // Save to the database
                var savedMenu = await _menuRepo.AddMenuAsync(menuDto);
                return savedMenu;
            }
            else
            {
                throw new Exception($"Error calling OpenAI API: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }
        }

    }
}
