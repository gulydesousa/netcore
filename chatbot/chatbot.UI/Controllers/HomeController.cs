using chatbot.Core.Interfaces;
using chatbot.UI.Models;
using chatbot.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace chatbot.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ChatbotApplicationService _chatbotApplicationService;

        public HomeController(ILogger<HomeController> logger, ChatbotApplicationService chatbotApplicationService)
        {
            _logger = logger;
            _chatbotApplicationService = chatbotApplicationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string message, string theme)
        {
            var botMessage = await _chatbotApplicationService.GenerateResponseAsync(message, theme);
            return View("Index", botMessage);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
