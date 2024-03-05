using chatbot.Core.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;


namespace chatbot.Infrastructure
{
    public class ChatGPTService : IChatbotService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ChatGPTService(HttpClient httpClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClient;           
        }

        public string GetApiKey()
        {
            return _configuration["ChatGPT:ApiKey"]??string.Empty;
        }

        public async Task<string> GenerateResponseAsync(string message, string theme)
        {
            string result = string.Empty;

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GetApiKey());

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                        new { role = "system", content = $"You're a {theme} instructor" },
                        new { role = "user", content = message }
                    }
            };

            request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                result = responseData != null ? responseData.choices[0].message.content : string.Empty;
                //aplicar formato html a este resultado

                return result;
            }
            else
            {
                throw new Exception($"Error calling ChatGPT API: {response.StatusCode}");
            }
        }
    }
}
