using CRUDexample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDexample.Filters.ActionFilters
{
    public class PersonsListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonsListActionFilter> _logger;

        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{FilterName}.{Action} method called"
                , typeof(PersonsListActionFilter)
                , nameof(OnActionExecuted));

            //Acceso al ViewBag del PersonsController
            PersonsController personsController = (PersonsController)context.Controller;

            IDictionary<string, object?>? arguments =
                (IDictionary<string, object?>?)context.HttpContext.Items["arguments"];
         
            if (arguments != null)
            {
                if (arguments.ContainsKey("ddlSearchBy"))
                {
                    personsController.ViewData["CurrentSearchBy"]
                        = Convert.ToString(arguments["ddlSearchBy"]);
                }

                if (arguments.ContainsKey("txtSearch"))
                {
                    personsController.ViewData["CurrentSearchText"]
                        = Convert.ToString(arguments["txtSearch"]);
                }

                if (arguments.ContainsKey("sortBy"))
                {
                    personsController.ViewData["CurrentSortBy"]
                        = Convert.ToString(arguments["sortBy"]);
                }
                else
                {
                    personsController.ViewData["CurrentSortBy"]
                                          = nameof(PersonResponse.PersonName);
                }

                if (arguments.ContainsKey("sortOrder"))
                {  
                    if (Enum.TryParse(typeof(SortOrderOptions), arguments["sortOrder"]?.ToString(), out var sortOrder))
                    {
                        personsController.ViewData["CurrentSortOrder"] = sortOrder;
                    }
                    else
                    {
                        personsController.ViewData["CurrentSortOrder"] = SortOrderOptions.ASC;
                    }
                }
                else
                {
                    personsController.ViewData["CurrentSortOrder"] = SortOrderOptions.ASC;
                }
            }
            
            personsController.ViewBag.SearchFields = SearchFieldsOptions();
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

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items.Add("arguments", context.ActionArguments);

            _logger.LogInformation("{FilterName}.{Action} method called"
                , typeof(PersonsListActionFilter)
                , nameof(OnActionExecuting));

            //Para tener acceso a los argumentos del método que se está ejecutando
            //Los leeremos y validaremos de la propiedad ActionArguments de la clase ActionExecutingContext
            if (context.ActionArguments.ContainsKey("ddlSearchBy"))
            {
                string? searchBy = Convert.ToString(context.ActionArguments["ddlSearchBy"]);
                string? txtSearch = Convert.ToString(context.ActionArguments["txtSearch"]);

                if(string.IsNullOrEmpty(txtSearch))
                {
                    context.ActionArguments["txtSearch"] =string.Empty;
                }

                if (!string.IsNullOrEmpty(searchBy))
                {
                    //Comprobaremos que su valor corresponde con alguna de las columnas de PersonResponse
                    List<string> SearchFieldsOptions = new List<string> { nameof(PersonResponse.PersonName)
                                                                                   , nameof(PersonResponse.Email)
                                                                                   , nameof(PersonResponse.DateOfBirth)
                                                                                   , nameof(PersonResponse.Gender)
                                                                                   , nameof(PersonResponse.Country)
                                                                                   , nameof(PersonResponse.Address)
                                                                                   , nameof(PersonResponse.Age)  };

                    //Usamos este formato para tener provecho del SEQ log
                    _logger.LogInformation("{argument} actual value:{value}", "searchBy", searchBy);
                    
                    if (!SearchFieldsOptions.Any(v => v == searchBy))
                    {
                        context.ActionArguments["ddlSearchBy"] = nameof(PersonResponse.PersonName);

                        _logger.LogInformation("{argument} updated value:{value}", "searchBy", context.ActionArguments["ddlSearchBy"]);
                    }
                }

            }
        }
    }
}
