﻿using chatbot.Core.Interfaces;
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
            string apikey = _configuration["ChatGPT:ApiKey"] ?? string.Empty;

            if (string.IsNullOrEmpty(apikey))
                throw new Exception("ChatGPT API Key is missing");
            else
                return apikey;
        }

        private string content(string theme)
        {
            string result = $"You're a {theme} programming language instructor";
            switch (theme)
            {
                case "About Sports":
                    result = $"You are an expert  {theme} world records";
                    break;
                case "Traductor":
                    result = $"You are a english translator";
                    break;
                case "Poeta":      
                    result = $"Eres un poeta de origen Japones que escribe hermosos microrelatos poeticos";
                    break;
                case "Image Generator":

                    break;
                default:
                    break;
            }

            return result;
        }


        /// <summary>
        /// The Chat Completions example highlights just one area of strength for our models: creative ability. 
        /// Explaining recursion (the programming topic) in a well formatted poem is something both the best developers and best poets would struggle with. 
        /// In this case, gpt-3.5-turbo does it effortlessly.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="theme"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> GenerateResponseAsync(string message, string theme)
        {
            string result = string.Empty;

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GetApiKey());

            var requestBody = new
            {
                //ID of the model to use. See the model endpoint compatibility table for details on which models work with the Chat API.
                model = "gpt-3.5-turbo",
                //A list of messages comprising the conversation so far. 
                messages = new[]
                {
                        new { role = "system", content = content(theme)},
                        new { role = "user", content = message }
                    },

                //How many chat completion choices to generate for each input message.
                //Note that you will be charged based on the number of generated tokens across all of the choices.
                //Keep n as 1 to minimize costs.
                n = (theme == "Traductor" ? 3 : 1),

                #region parameters
                //Number between -2.0 and 2.0.
                //Positive values penalize new tokens based on their existing frequency in the text so far,
                //decreasing the model's likelihood to repeat the same line verbatim.
                //frequency_penalty = 0,

                //Modify the likelihood of specified tokens appearing in the completion.
                //Accepts a JSON object that maps tokens(specified by their token ID in the tokenizer)
                //to an associated bias value from - 100 to 100.Mathematically,
                //the bias is added to the logits generated by the model prior to sampling.
                //The exact effect will vary per model,
                //but values between - 1 and 1 should decrease or increase likelihood of selection;
                //values like -100 or 100 should result in a ban or exclusive selection of the relevant token.
                //logit_bias = new {},

                //Whether to return log probabilities of the output tokens or not.
                //If true, returns the log probabilities of each output token returned in the content of message.
                //This option is currently not available on the gpt-4-vision-preview model.
                //logprobs = new {},

                //An integer between 0 and 20 specifying the number of most likely tokens to return at each token position,
                //each with an associated log probability.
                //logprobs must be set to true if this parameter is used.
                //top_logprobs = new {},

                //The maximum number of tokens that can be generated in the chat completion.
                //max_tokens = new {},

                //Number between -2.0 and 2.0.
                //Positive values penalize new tokens based on whether they appear in the text so far,
                //increasing the model's likelihood to talk about new topics.
                //presence_penalty = new {},

                //An object specifying the format that the model must output.
                //Compatible with GPT-4 Turbo and all GPT-3.5 Turbo models newer than gpt-3.5-turbo-1106.
                //Setting to { "type": "json_object" } enables JSON mode, which guarantees the message the model generates is valid JSON.
                //Important: when using JSON mode, you must also instruct the model to produce JSON yourself via a system or user message.
                //Without this, the model may generate an unending stream of whitespace until the generation reaches the token limit,
                //resulting in a long-running and seemingly "stuck" request.
                //Also note that the message content may be partially cut off if finish_reason = "length",
                //which indicates the generation exceeded max_tokens or the conversation exceeded the max context length.
                //response_format = "text",

                //This feature is in Beta.
                //If specified, our system will make a best effort to sample deterministically,
                //such that repeated requests with the same seed and parameters should return the same result.
                //Determinism is not guaranteed,
                //and you should refer to the system_fingerprint response parameter to monitor changes in the backend.
                //seed = new { },

                //Up to 4 sequences where the API will stop generating further tokens.
                //stop = new {},

                //If set, partial message deltas will be sent, like in ChatGPT.
                //Tokens will be sent as data-only server-sent events as they become available,
                //with the stream terminated by a data: [DONE]
                //stream = false,

                //What sampling temperature to use, between 0 and 2.
                //Higher values like 0.8 will make the output more random,
                //while lower values like 0.2 will make it more focused and deterministic.
                //We generally recommend altering this or top_p but not both.
                //temperature = new {},

                //An alternative to sampling with temperature, called nucleus sampling,
                //where the model considers the results of the tokens with top_p probability mass.
                //So 0.1 means only the tokens comprising the top 10% probability mass are considered.
                //We generally recommend altering this or temperature but not both.
                //top_p = new {},

                //A list of tools the model may call.
                //Currently, only functions are supported as a tool.
                //Use this to provide a list of functions the model may generate JSON inputs for.
                //A max of 128 functions are supported.
                //tools = new {},

                //Controls which (if any) function is called by the model.
                //none means the model will not call a function and instead generates a message.
                //auto means the model can pick between generating a message or calling a function.
                //Specifying a particular function via {"type": "function", "function": {"name": "my_function"}}
                //forces the model to call that function.
                //none is the default when no functions are present.
                //auto is the default if functions are present.
                //tool_choice = "none",

                //A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse. 
                //user = new { },
                #endregion parameters
            };
            request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                try
                {
                    var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                    foreach (var item in responseData!.choices)
                    {
                        var messageContent = item.message.content;
                        var messageRole = item.message.role;
                        result += $"<p>{messageContent}</p>";
                    }
                }
                catch (JsonException ex)
                {
                    throw new Exception("Error deserializing ChatGPT API response", ex);
                }

                //aplicar formato html a este resultado
                return result;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error calling ChatGPT API: {response.StatusCode}. Response: {errorContent}");
            }
        }


        public async Task<string> CallDallEApiAsync(string input)
        {
            string imageUrl = string.Empty;

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/images/generations");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GetApiKey());

            var requestBody = new
            {
                model = "dall-e-3",
                prompt = input,
                n = 1,
                size = "1024x1024"
            };
            request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                // Acceder a la propiedad "url"
                if (responseData != null)  imageUrl = responseData.data[0].url;

                return imageUrl;
            }
            else
            {
                throw new Exception($"Error calling DALL-E API: {response.StatusCode}");
            }
        }

    }
}
