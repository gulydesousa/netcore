using System.ComponentModel.DataAnnotations;


namespace ContactManager.Core.DTO
{
    /// <summary>
    /// Represents the data transfer object for login information.
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
