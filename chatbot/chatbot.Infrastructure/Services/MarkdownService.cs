using chatbot.Core.Interfaces;
using Markdig;


namespace chatbot.Infrastructure.Services
{
    public class MarkdownService : IMarkdownService
    {
        public string ConvertToHtml(string markdownText)
        {
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            string html = Markdown.ToHtml(markdownText, pipeline);
            return html;
        }
    }
}
