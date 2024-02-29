using CRUDexample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;

namespace CRUDexample.Filters.ActionFilters
{
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
    {
        private readonly ICountriesService _countriesService;

        public PersonCreateAndEditPostActionFilter(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context
                    , ActionExecutionDelegate next)
        {
            //Before logic
            if (context.Controller is PersonsController personsController)
            {
                //Cada controller tiene un ModelState
                if (!personsController.ModelState.IsValid)
                {
                    List<CountryResponse> countries =
                    (List<CountryResponse>)await _countriesService.GetCountries();

                    personsController.ViewBag.Countries = countries.Select(opt =>
                    new SelectListItem()
                    {
                        Text = opt.CountryName,
                        Value = opt.CountryId.ToString()
                    }).ToList(); ;

                    personsController.ViewBag.Errors = personsController.ModelState.Values
                                                .SelectMany(v => v.Errors)
                                                .Select(x => x.ErrorMessage).ToList();

                    //short-circuiting
                    var personRequest = context.ActionArguments["personRequest"];
                    context.Result = personsController.View(personRequest);
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }
            //After Logic
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
    }
}
