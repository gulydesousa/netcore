using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    /// <summary>
    /// Domain model for the County entity
    /// </summary>
    public class Country
    {
        /// <summary>
        /// Gets or sets the unique identifier of the country.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        [StringLength(40)] //nvarchar(40)
        public string? CountryName { get; set; }

        /// <summary>
        /// Gets or sets the collection of persons associated with the country.
        /// </summary>
        public virtual ICollection<Person>? Persons { get; set; }

        /// <summary>
        /// Returns the hash code for the country.
        /// </summary>
        /// <returns>The hash code for the country.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the country.
        /// </summary>
        /// <returns>A string that represents the country.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("CountryID: ").Append(Id).AppendLine();
            sb.Append("Country: ").Append(CountryName).AppendLine();

            return sb.ToString();
        }
    }
}