using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Person Domain Model Class
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Gets or sets the unique identifier of the person.
        /// </summary>
        [Key]
        public Guid PersonID { get; set; }

        /// <summary>
        /// Gets or sets the name of the person.
        /// </summary>
        [StringLength(40)] //nvarchar(40)
        public string? PersonName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the person.
        /// </summary>
        [StringLength(40)] //nvarchar(40)
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the date of birth of the person.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the gender of the person.
        /// </summary>
        [StringLength(10)] //nvarchar(10)
        public string? Gender { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the country.
        /// </summary>
        public Guid CountryID { get; set; }

        /// <summary>
        /// Gets or sets the address of the person.
        /// </summary>
        [StringLength(200)] //nvarchar(200)
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the person wants to receive newsletters.
        /// </summary>
        public bool ReceiveNewsLetter { get; set; }

        /// <summary>
        /// Gets or sets the Tax Identification Number (TIN) of the person.
        /// </summary>
        [StringLength(15)] //nvarchar(15)
        public string? TIN { get; set; }

        /// <summary>
        /// Gets or sets the country associated with the person.
        /// </summary>
        [ForeignKey("CountryID")]
        public virtual Country? Country { get; set; }

        /// <summary>
        /// Returns a string that represents the current person.
        /// </summary>
        /// <returns>A string that represents the current person.</returns>
        public override string ToString()
        {
            return $"PersonID: {PersonID}, PersonName: {PersonName}, " +
                $"Email: {Email}, DateOfBirth: {DateOfBirth?.ToString("dd/MM/yyyy")}, " +
                $"Gender: {Gender}, CountryID: {CountryID}, " +
                $"CountryName {Country?.CountryName}, Address: {Address}, " +
                $"ReceiveNewsLetter: {ReceiveNewsLetter}, TIN: {TIN}";
        }
    }
}
