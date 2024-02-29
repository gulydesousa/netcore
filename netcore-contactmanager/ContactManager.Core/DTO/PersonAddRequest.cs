using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Acts as a DTO for inserting a new person
    /// </summary>
    public class PersonAddRequest
    {
        /// <summary>
        /// Gets or sets the person's name.
        /// </summary>
        [Required(ErrorMessage = "Person Name is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Person Name must not be empty")]
        public string? PersonName { get; set; }

        /// <summary>
        /// Gets or sets the person's email.
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the person's date of birth.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the person's gender.
        /// </summary>
        [Required(ErrorMessage = "Select a gender")]
        public GenderOptions? Gender { get; set; }

        /// <summary>
        /// Gets or sets the person's country ID.
        /// </summary>
        [Display(Name = "Country")]
        [Required(ErrorMessage = "Country is required")]
        public Guid CountryID { get; set; }

        /// <summary>
        /// Gets or sets the person's address.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the person wants to receive newsletters.
        /// </summary>
        public bool ReceiveNewsLetter { get; set; }

        /// <summary>
        /// Converts the current instance of PersonAddRequest into a new object of Person type.
        /// </summary>  
        /// <returns>A new instance of Person with the same property values as the current instance of PersonAddRequest.</returns>
        public Person ToPerson()
        {
            return new Person()
            {
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
