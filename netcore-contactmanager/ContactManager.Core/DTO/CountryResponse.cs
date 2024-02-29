using Entities;

namespace ServiceContracts.DTO
{
    ///<summary>
    /// DTO class that is used as the return type for most of the methods in the CountriesService.
    ///</summary>
    public class CountryResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the country.
        /// </summary>
        public Guid CountryId { get; set; }

        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        public string? CountryName { get; set; }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// Returns true if both values are equal; otherwise, false.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>True if both values are equal; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(CountryResponse))
            {
                return false;
            }

            CountryResponse countryResponse = (CountryResponse)obj;

            return this.CountryId == countryResponse.CountryId
                && this.CountryName == countryResponse.CountryName;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Converts a Country object to a CountryResponse object.
        /// </summary>
        /// <param name="country">The Country object to convert.</param>
        /// <returns>A new instance of the CountryResponse class.</returns>
        public static CountryResponse ToCountryResponse(Country country)
        {
            return new CountryResponse
            {
                CountryId = country.Id,
                CountryName = country.CountryName
            };
        }
    }

    /// <summary>
    /// Provides extension methods for the Country class.
    /// </summary>
    public static class CountryExtensions
    {
        /// <summary>
        /// Converts a Country object to a CountryResponse object.
        /// </summary>
        /// <param name="country">The Country object to convert.</param>
        /// <returns>A new instance of the CountryResponse class.</returns>
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse
            {
                CountryId = country.Id,
                CountryName = country.CountryName
            };
        }
    }
}
