using ContactManager.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace ContactManager.Core.DTO
{
    /// <summary>
    /// Represents the data transfer object for registering a user.
    /// </summary>
    public class RegisterDTO
    {
        /// <summary>
        /// Gets or sets the person's name.
        /// </summary>
        [Required(ErrorMessage = "Name can't be blank")]
        public string PersonName { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "Email is already registered")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        [Required(ErrorMessage = "Phone can't be blank")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number should contain numbers only")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        [Required(ErrorMessage = "Confirm Password can't be blank")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirm password don't match")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the user type.
        /// </summary>
        public UserTypeOptions UserType { get; set; } = UserTypeOptions.User;
    }
}
