
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using Sotex.Api.Interfaces;
using Sotex.Api.Model;
using Newtonsoft.Json;

namespace Sotex.Api.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAIService(HttpClient httpClient, IOptions<OpenAISettings> options)
        {
            _httpClient = httpClient;
            _apiKey = options.Value.ApiKey;
        }

        public async Task<string> ParseImageContentAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded");
            }

            var openAiApiUrl = "https://api.openai.com/v1/images/generations"; // Change as needed for the specific endpoint

            using var content = new MultipartFormDataContent();
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0; // Reset stream position to the beginning

            // Add the image to the content
            content.Add(new StreamContent(stream), "file", file.FileName);

            // Set up the authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            // Make the request to OpenAI API
            var response = await _httpClient.PostAsync(openAiApiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic parsedResponse = JsonConvert.DeserializeObject(responseContent);
                return parsedResponse.choices[0].text; // Adjust based on actual API response structure
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception($"OpenAI API request failed: {errorResponse}");
            }
        }
    }
}
