using chatbot.Core.Interfaces;

namespace chatbot.UI.Services
{
    public class ChatbotApplicationService
    {
        private readonly IChatbotService _chatbotService;
        private readonly IMarkdownService _markdownService;
        private readonly ILanguagesService _languages;

        public ChatbotApplicationService(IChatbotService chatbotService
                                       , IMarkdownService markdownService
                                       , ILanguagesService languages)
        {
            _chatbotService = chatbotService;
            _markdownService = markdownService;
            _languages = languages;
        }

        public async Task<string> GenerateResponseAsync(string message, string theme)
        {
            string result = await _chatbotService.GenerateResponseAsync(message, theme);
            result = _markdownService.ConvertToHtml(result);
            return result;
        }

        public async Task<string> CallDallEApiAsync(string message)
        {
            string result = await _chatbotService.CallDallEApiAsync(message);
            return result;
        }



        public async Task<List<string>> GetLanguagesAsync()
        {
            List<string> languages = new List<string>();
            languages = await _languages.GetLanguagesAsync();
            return languages?? new List<string>();
        }
    }
}
