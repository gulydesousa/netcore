using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class for adding a new country
    /// </summary>
    public class CountryAddRequest
    {
        /// <summary>
        /// Gets or sets the country name.
        /// </summary>
        public string? CountryName { get; set; }

        /// <summary>
        /// Converts the CountryAddRequest object to a Country object.
        /// </summary>
        /// <returns>The converted Country object.</returns>
        public Country ToCountry()
        {
            return new Country
            {
                CountryName = CountryName
            };
        }
    }
}