namespace chatbot.Core.Interfaces
{
    public interface IChatbotService
    {
        Task<string> GenerateResponseAsync(string message, string theme);
    }
}
