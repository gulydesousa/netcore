using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CRUDexample.Controllers
{
    /// <summary>
    /// Represents the controller for the home page.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Handles the error page.
        /// </summary>
        /// <returns>The error view.</returns>
        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error()
        {
            IExceptionHandlerPathFeature? exceptionHandlerPathFeature =
            HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature != null &&
                exceptionHandlerPathFeature.Error != null)
            {
                ViewBag.ErrorMessage = exceptionHandlerPathFeature.Error.Message;
            }
            return View();
        }
    }
}
