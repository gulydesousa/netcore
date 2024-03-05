using chatbot.Core.Interfaces;

namespace chatbot.UI.Services
{
    public class ChatbotApplicationService
    {
        private readonly IChatbotService _chatbotService;
        private readonly IMarkdownService _markdownService;

        public ChatbotApplicationService(IChatbotService chatbotService, IMarkdownService markdownService)
        {
            _chatbotService = chatbotService;
            _markdownService = markdownService;
        }

        public async Task<string> GenerateResponseAsync(string message, string theme)
        {
            string result = await _chatbotService.GenerateResponseAsync(message, theme);
            result = _markdownService.ConvertToHtml(result);
            return result;


        }
    }
}
