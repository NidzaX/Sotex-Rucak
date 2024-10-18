﻿
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

        public async Task<Menu> ParseAndSaveMenuFromFileAsync(IFormFile file, string purpose)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File cannot be null or empty.");
            }

            // Convert the image to base64
            string base64Image;
            using (var fileStream = file.OpenReadStream())
            {
                base64Image = _resizeImage.Resize(fileStream, maxWidth: 800, maxHeight: 800);
            }

            // Prepare request for OpenAI API with a specific prompt for consistent JSON structure
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
                        "    \"phone\": \"string\"" +
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

            // Send request to OpenAI API
            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<ResponseDto>(responseContent);

                var content = jsonResponse.choices[0].message.content.ToString().Trim();

                // Extract JSON from the response using regex
                var jsonRegex = new Regex(@"\{.*\}", RegexOptions.Singleline);
                var match = jsonRegex.Match(content);

                if (match.Success)
                {
                    string jsonPart = match.Value;
                    Console.WriteLine("Extracted JSON: " + jsonPart);  // Log extracted JSON

                    // Deserialize JSON into AddMenuDto
                    var menuData = JsonConvert.DeserializeObject<AddMenuDto>(jsonPart);

                    if (menuData == null)
                    {
                        throw new Exception("Failed to deserialize menu data or menu is null.");
                    }
                    // Map to database model
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
                        }
                    };

                    // Save to database
                    var savedMenu = await _menuRepo.AddMenuAsync(menuDto);
                    return savedMenu;
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


    }
}
