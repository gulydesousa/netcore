using ContactsManager.UI.Utilities;
using CRUDexample.Filters;
using CRUDexample.Filters.ActionFilters;
using CRUDexample.Filters.AuthorizationFilter;
using CRUDexample.Filters.ResouceFilters;
using CRUDexample.Filters.ResultFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
namespace CRUDexample.Controllers
{
    [Route("[controller]")]
    //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "X-Custom-Controller-Key", "Custom-Controller-Value", 3 }
    //, Order = 3)]
    [ResponseHeaderFilterFactory("X-Custom-Controller-Key", "Custom-Controller-Value", 3)]
    //[TypeFilter(typeof(HandleExceptionFilter))]
    [TypeFilter(typeof(PersonAlwaysRunResultFilter))]
    public class PersonsController : Controller
    {
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IPersonsSorterService _personsSorterService;
        private readonly IPersonsUpdaterService _personsUpdaterService;

        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonsController> _logger;

        //constructor injection
        public PersonsController(
            IPersonsAdderService personsAdderService, 
            IPersonsDeleterService personsDeleterService, 
            IPersonsGetterService personsGetterService, 
            IPersonsSorterService personsSorterService, 
            IPersonsUpdaterService personsUpdaterService,
            ICountriesService countriesService,
            ILogger<PersonsController> logger)
        {
            _personsAdderService = personsAdderService;
            _personsDeleterService = personsDeleterService;
            _personsGetterService = personsGetterService;
            _personsSorterService = personsSorterService;
            _personsUpdaterService = personsUpdaterService;
            _countriesService = countriesService;
            _logger = logger;
        }


        //Url: persons/index
        [Route("[action]")]
        [Route("/")]
        [ServiceFilter(typeof(PersonsListActionFilter), Order = 4)]
        //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "X-Index-Custom-ActionMethod-Key", "Custom-Index-Custom-ActionMethod-Value", 1 }
        //, Order = 1)]
        [ResponseHeaderFilterFactory("X-Index-Custom-ActionMethod-Key", "Custom-Index-Custom-ActionMethod-Value", 1)]
        [TypeFilter(typeof(PersonsListResultFilter))]
        [SkipFilter]
        // @*Eg: ddlSearchBy=PersonName&txtSearch=John&btnSearch=Search*@
        public async Task<IActionResult> Index(string ddlSearchBy, string txtSearch
                                    , string sortBy = nameof(PersonResponse.PersonName)
                                    , SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            _logger.LogInformation("PersonsController.Index() called");

            string sanitizedDdlSearchBy = ddlSearchBy?.Replace(Environment.NewLine, "");
            string sanitizedTxtSearch = txtSearch?.Replace(Environment.NewLine, ""); 
            string sanitizedSortBy = sortBy?.Replace(Environment.NewLine, ""); 
            string sanitizedSortOrder = sortOrder.ToString().Replace(Environment.NewLine, ""); 


            _logger.LogDebug($"ddlSearchBy:{sanitizedDdlSearchBy}, txtSearch:{sanitizedTxtSearch}, sortBy:{sanitizedSortBy}, sortOrder:{sanitizedSortOrder}");

            //Search
            List<PersonResponse> persons =
            await _personsGetterService.GetFilteredPersons(ddlSearchBy??string.Empty, txtSearch);

            //Sorting
            persons = _personsSorterService.GetSortedPersons(persons, sortBy??string.Empty, sortOrder);
            return View(persons);
        }


        //Executes when user clicks on "Create Person" hyperlink (while opening the create view) 
        //Url: persons/create
        [Route("[action]")]
        [HttpGet]
        //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "X-Create-Custom-ActionMethod-Key", "Custom-Create-Custom-ActionMethod-Value", 1 }
        //, Order = 1)]
        [ResponseHeaderFilterFactory("X-Create-Custom-ActionMethod-Key", "Custom-Create-Custom-ActionMethod-Value", 1)]
        public async Task<IActionResult> Create()
        {
            PersonAddRequest personAddRequest = new PersonAddRequest();

            ViewBag.Countries = await GetCountrySelectListItems(personAddRequest.CountryID);

            return View(personAddRequest);
        }

        //Executes when user clicks on "Create" button (while submitting the create form)
        //Url: persons/create
        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        //[TypeFilter(typeof(FeatureDisabledResourceFilter), Arguments = new object[] { true })]
        public async Task<IActionResult> Create(PersonAddRequest personRequest)
        {
            await _personsAdderService.AddPerson(personRequest);
            return RedirectToAction("Index", "Persons");
        }

        //Action method to edit
        //Url: persons/edit/{personID}
        [HttpGet]
        [Route("[action]/{personID}")]
        [TypeFilter(typeof(TokenResultFilter))]
        public async Task<IActionResult> Edit(Guid personID)
        {
            PersonResponse? personResponse =
            await _personsGetterService.GetPersonByPersonID(personID);

            if (personResponse == null)
            {
                return RedirectToAction("Index", "Persons");
            }
            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            ViewBag.Countries = await GetCountrySelectListItems(personUpdateRequest.CountryID);

            return View(personUpdateRequest);
        }


        //Action method to edit
        //Url: persons/edit/{personID}
        [HttpPost]
        [Route("[action]/{personID}")]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<IActionResult> Edit(PersonUpdateRequest personRequest)
        {
            PersonResponse? personResponse
            = await _personsGetterService.GetPersonByPersonID(personRequest.PersonID);

            if (personResponse == null)
            {
                return RedirectToAction("Index", "Persons");
            }

            PersonResponse updatedPerson =
            await _personsUpdaterService.UpdatePerson(personRequest);

            return RedirectToAction("Index", "Persons");
        }


        //Action method to delete
        //Url: persons/delete/{personID}
        [HttpGet]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(Guid personID)
        {
            PersonResponse? personResponse =
            await _personsGetterService.GetPersonByPersonID(personID);

            if (personResponse == null)
            {
                return RedirectToAction("Index", "Persons");
            }

            return View(personResponse);
        }


        //Action method to delete
        //Url: persons/delete/{personID}
        [HttpPost]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? _personResponse = await _personsGetterService
                .GetPersonByPersonID(personUpdateRequest.PersonID);

            if (_personResponse == null)
            {
                return RedirectToAction("Index", "Persons");
            }
            else
            {
                bool result =
                    await _personsDeleterService.DeletePerson(personUpdateRequest.PersonID);
                return RedirectToAction("Index", "Persons");

            }
        }


        private Dictionary<string, string> SearchFieldsOptions()
        {
            //Dictionary with PersonResponse property name and column display name to seach
            Dictionary<string, string> searchFields = new Dictionary<string, string>{
                        {nameof(PersonResponse.PersonName), "Person Name"},
                        {nameof(PersonResponse.Email), "Email"},
                        {nameof(PersonResponse.DateOfBirth), "Date of Birth" },
                        {nameof(PersonResponse.Age), "Age" },
                        {nameof(PersonResponse.Gender), "Gender" },
                        {nameof(PersonResponse.Country), "Country" },
                        {nameof(PersonResponse.Address), "Address" },
                        {nameof(PersonResponse.ReceiveNewsLetter), "Receive News Letter" }
                    };

            return searchFields;
        }

        private async Task<List<SelectListItem>> GetCountrySelectListItems(Guid selectedCountryId)
        {
            IList<CountryResponse> countries = await _countriesService.GetCountries();


            return countries.Select(opt =>
                new SelectListItem()
                {
                    Text = opt.CountryName,
                    Value = opt.CountryId.ToString(),
                    Selected = (selectedCountryId == opt.CountryId)
                }).ToList();
        }

        //Action method to export to PDF
        //Url: persons/PersonsPDF
        [Route("PersonsPDF")]
        public async Task<IActionResult> PersonsPDF()
        {
            List<PersonResponse> persons = await _personsGetterService.GetAllPersons();
            ViewBag.SearchFields = SearchFieldsOptions();

            return new ViewAsPdf("PersonsPDF", persons, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Bottom = 20, Left = 20, Right = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        //Action method to export to CSV
        //Url: persons/PersonsCSV
        [Route("PersonsCSV")]
        public async Task<IActionResult> PersonsCSV()
        {
            MemoryStream ms = await _personsGetterService.GetPersonsCSV();
            return File(ms, "application/octet-stream", "persons.csv");
        }

        //Action method to export to EXCEL
        //Url: persons/PersonsEXCEL
        [Route("PersonsEXCEL")]
        public async Task<IActionResult> PersonsEXCEL()
        {
            MemoryStream ms = await _personsGetterService.GetPersonsEXCEL();
            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }

    }
}
