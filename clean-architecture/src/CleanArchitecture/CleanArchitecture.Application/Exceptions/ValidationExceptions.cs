namespace CleanArchitecture.Application.Exceptions;
public sealed class ValidationExceptions : Exception
{
    public IEnumerable<ValidationError> Errors { get; }

    public ValidationExceptions(IEnumerable<ValidationError> errors)     
    {
        Errors = errors;
    }
}
