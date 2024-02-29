using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repository
{
    /// <summary>
    /// Repository class for managing countries.
    /// </summary>
    public class CountriesRepository : ICountriesRepository
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountriesRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The application database context.</param>
        public CountriesRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Adds a new country to the database.
        /// </summary>
        /// <param name="country">The country to add.</param>
        /// <returns>The added country.</returns>
        public async Task<Country> AddCountry(Country country)
        {
            _dbContext.Add(country);
            await _dbContext.SaveChangesAsync();
            return country;
        }

        /// <summary>
        /// Retrieves all countries from the database.
        /// </summary>
        /// <returns>A list of countries.</returns>
        public async Task<List<Country>> GetCountries()
        {
            return await _dbContext.Countries.ToListAsync();
        }

        /// <summary>
        /// Retrieves a country by its ID.
        /// </summary>
        /// <param name="countryId">The ID of the country.</param>
        /// <returns>The country with the specified ID, or null if not found.</returns>
        public async Task<Country> GetCountryById(Guid? countryId)
        {
            return await _dbContext.Countries.FirstOrDefaultAsync(c => c.Id == countryId);
        }

        /// <summary>
        /// Retrieves a country by its name.
        /// </summary>
        /// <param name="countryName">The name of the country.</param>
        /// <returns>The country with the specified name, or null if not found.</returns>
        public async Task<Country?> GetCountryByName(string countryName)
        {
            return await _dbContext.Countries.FirstOrDefaultAsync(c => c.CountryName == countryName);
        }
    }
}