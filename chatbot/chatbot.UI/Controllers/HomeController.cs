using chatbot.UI.Models;
using chatbot.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Xml.Linq;

namespace chatbot.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ChatbotApplicationService _chatbotApplicationService;
       



        public List<string> themes = new List<string>();

        public HomeController(ILogger<HomeController> logger, ChatbotApplicationService chatbotApplicationService)
        {
            _logger = logger;
            _chatbotApplicationService = chatbotApplicationService;

            // Inicializa 'themes' en un método separado para evitar bloquear el hilo principal
            InitializeThemes();
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

            botMessage = $"<h3><i>\"{model.Message}\"</i></h3>" + botMessage; // Include the initial question as an h2 in botMessage with italics

            return View("Index", botMessage);
        }

        private async void InitializeThemes()
        {
            themes = await _chatbotApplicationService.GetLanguagesAsync();
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
