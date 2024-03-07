

using chatbot.Core.Interfaces;

namespace chatbot.Infrastructure
{
    public class LanguagesService : ILanguagesService
    {
        public Task<List<string>> GetLanguagesAsync()
        {
            return Task.FromResult(new List<string> { "Python", "C#", "JavaScript", "Java", "Angular" });

        }
    }
}
