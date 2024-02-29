using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents the DTO class that contains the person details to be updated
    /// </summary>
    public class PersonUpdateRequest
    {
        /// <summary>
        /// Gets or sets the person ID.
        /// </summary>
        [Required(ErrorMessage = "Person ID is required")]
        public Guid? PersonID { get; set; }

        /// <summary>
        /// Gets or sets the person name.
        /// </summary>
        [Required(ErrorMessage = "Person Name is required")]
        public string? PersonName { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        public GenderOptions? Gender { get; set; }

        /// <summary>
        /// Gets or sets the country ID.
        /// </summary>
        public Guid CountryID { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the person wants to receive newsletters.
        /// </summary>
        public bool ReceiveNewsLetter { get; set; }

        /// <summary>
        /// Converts the current instance of PersonUpdateRequest into a new object of Person type.
        /// </summary>
        /// <returns>A new instance of Person with the updated details.</returns>
        public Person ToPerson()
        {
            return new Person()
            {
                PersonID = PersonID ?? default(Guid),
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                CountryID = CountryID,
                Address = Address,
                ReceiveNewsLetter = ReceiveNewsLetter
            };
        }
    }
}
