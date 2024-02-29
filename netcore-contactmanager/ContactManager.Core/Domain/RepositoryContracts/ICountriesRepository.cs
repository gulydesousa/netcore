using Entities;

namespace RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managind Person Entity
    /// </summary>
    public interface ICountriesRepository
    {

        /// <summary>
        /// Adds a new Country object to data store
        /// </summary>
        /// <param name="country">Country object to add</param>
        /// <returns>Returns te country object after adding it to datastore</returns>
        Task<Country> AddCountry(Country country);

        /// <summary>
        /// Returns all countries in the data store
        /// </summary>
        /// <returns>Returns all countries from the table</returns>
        Task<List<Country>> GetCountries();

        /// <summary>
        /// Returns a country by its Id 
        /// </summary>
        /// <param name="countryId">Country ID to search</param>
        /// <returns>Matching Country otherwise null</returns>
        Task<Country> GetCountryById(Guid? countryId);

        /// <summary>
        /// Returns country by its name 
        /// </summary>
        /// <param name="countryName">Name of the country</param>
        /// <returns></returns>
        Task<Country?> GetCountryByName(string countryName);

    }
}