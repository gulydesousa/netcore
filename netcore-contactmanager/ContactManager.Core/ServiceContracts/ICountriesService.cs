using Microsoft.AspNetCore.Http;
using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents the business logic for the Countries entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds as country object ot the list of countries
        /// </summary>
        /// <param name="countryAddRequest"></param>
        /// <returns>Returns the object after adding it</returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// Returns a list of countries
        /// </summary>
        /// <returns>All countries from the lista as List of CountryResponse</returns>
        Task<IList<CountryResponse>> GetCountries();

        /// <summary>
        /// Returns a country object based on the id
        /// </summary>
        /// <param name="countryID">countryID (guid) to seach</param>
        /// <returns>Matching country as CountryResponse object</returns>
        Task<CountryResponse?> GetCountryById(Guid? countryID);

        /// <summary>
        /// Uploads countries from excel file into database
        /// </summary>
        /// <param name="formFile">Excel with a list of countries</param>
        /// <returns>Number of countries</returns>
        Task<int> UploadCountryFromExcelFile(IFormFile formFile);
    }
}