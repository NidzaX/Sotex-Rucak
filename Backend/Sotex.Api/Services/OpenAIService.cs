
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Sotex.Api.Interfaces;
using Sotex.Api.Model;
using Newtonsoft.Json;
using System.Text;

using Sotex.Api.Services.DependencyInjection;


namespace Sotex.Api.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private ResizeImage _resizeImage;
        public OpenAIService(HttpClient httpClient, IOptions<OpenAISettings> options, ResizeImage resizeImage)
        {
            _httpClient = httpClient;
            _apiKey = options.Value.ApiKey;
            _resizeImage = resizeImage;
        }

        public async Task<string> ParseImageFromFileAsync(IFormFile file, string purpose)
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

                dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);
                string content = jsonResponse.choices[0].message.content;

                var match = System.Text.RegularExpressions.Regex.Match(content, @"```json(.*?)```", System.Text.RegularExpressions.RegexOptions.Singleline);
                if (match.Success)
                {
                    string extractedJson = match.Groups[1].Value.Trim();
                    return extractedJson; 
                }

                return content;
            }
            else
            {
                throw new Exception($"Error calling OpenAI API: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }
        }
    }
}
