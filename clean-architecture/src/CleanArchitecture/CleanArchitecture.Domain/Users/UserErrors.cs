using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Users
{
    /// <summary>
    /// Represents a collection of user-related error messages.
    /// </summary>
    public static class UserErrors
    {
        //Los objetos de tipo Error son inmutables, por lo que se pueden reutilizar en toda la aplicación.
        //Se usa la convención de que los errores se definen como propiedades estáticas en una clase estática.
 
 
        /// <summary>
        /// Represents an error when a user is not found.
        /// </summary>
        public static Error NotFound = new Error(
            "User.NotFound",
            "No existe el usuario buscado por este id");

        /// <summary>
        /// Represents an error when invalid credentials are provided.
        /// </summary>
        public static Error InvalidCredentials = new Error(
            "User.InvalidCredentials",
            "Las credenciales ingresadas no son válidas");
    }
}
