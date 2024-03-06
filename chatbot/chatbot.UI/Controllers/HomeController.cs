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
        private List<string>  themes = new List<string> { "Python", "C#", "JavaScript", "Java", "Angular" };


        public HomeController(ILogger<HomeController> logger, ChatbotApplicationService chatbotApplicationService)
        {
            _logger = logger;
            _chatbotApplicationService = chatbotApplicationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Themes = themes;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ChatbotViewModel model)
        {
            var botMessage = await _chatbotApplicationService.GenerateResponseAsync(model.Message, model.Theme);

            ViewBag.Themes = themes;
            ViewBag.Theme = model.Theme; // Almacena el tema seleccionado en el ViewBag

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
