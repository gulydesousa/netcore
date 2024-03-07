
namespace chatbot.Core.Interfaces
{
    public interface ILanguagesService
    {
        Task<List<string>> GetLanguagesAsync();

    }
}
