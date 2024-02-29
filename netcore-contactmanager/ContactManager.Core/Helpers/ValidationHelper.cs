using System.ComponentModel.DataAnnotations;

namespace Services.Helpers
{
    public class ValidationHelper
    {
        /// <summary>
        /// Performs model validation using data annotations.
        /// </summary>
        /// <param name="obj">The object to be validated.</param>
        /// <exception cref="ArgumentException">Thrown if the object fails validation.</exception>
        public static void ModelValidation(object obj)
        {
            // Model Validations
            ValidationContext context = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(obj, context, validationResults, true);
            if (!isValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
