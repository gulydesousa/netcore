using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace CRUDexample.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    {

        private readonly ICountriesService _countriesService;

        //constructor injection
        public CountriesController(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }

        /// <summary>
        /// Displays the view for uploading data from an Excel file.
        /// </summary>
        /// <returns>The view for uploading data from an Excel file.</returns>
        [Route("UploadFromExcel")]
        public IActionResult UploadFromExcel()
        {
            return View();
        }

        /// <summary>
        /// Handles the POST request for uploading data from an Excel file.
        /// </summary>
        /// <param name="excelFile">The Excel file to upload.</param>
        /// <returns>The view with the result message.</returns>
        [HttpPost]
        [Route("UploadFromExcel")]
        public async Task<IActionResult> UploadFromExcel(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ViewBag.ErrorMessage = "Please select an excel file";
                return View();
            }

            if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.ErrorMessage = "Unsupported file. 'xlsx' file is expected";
                return View();
            }

            int inserted =
            await _countriesService.UploadCountryFromExcelFile(excelFile);

            ViewBag.Message = $"{inserted} Countries Uploaded";
            return View();
        }
    }
}
