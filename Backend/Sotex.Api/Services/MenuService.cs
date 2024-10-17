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
                            new
                            {
                                type = "image_url",
                                image_url = new
                                {
                                    url = $"data:image/jpeg;base64,{base64Image}"
                                }
                            }
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
                Console.WriteLine("Response from OpenAI API: " + responseContent);

                dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);
                string content = jsonResponse.choices[0].message.content;

                var match = System.Text.RegularExpressions.Regex.Match(content, @"{.*}", System.Text.RegularExpressions.RegexOptions.Singleline);
                if (match.Success)
                {
                    string extractedJson = match.Groups[0].Value.Trim();
                    var menuDto = JsonConvert.DeserializeObject<AddMenuDto>(extractedJson);
                    var menuEntity = _mapper.Map<Menu>(menuDto);
                    try
                    {
                        var savedMenu = await _menuRepo.AddMenuAsync(menuEntity); // Use async version
                        return savedMenu;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    return null;
                }
                throw new Exception("No valid JSON found in response.");
            }
            else
            {
                throw new Exception($"Error calling OpenAI API: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }
        }

    }
}
