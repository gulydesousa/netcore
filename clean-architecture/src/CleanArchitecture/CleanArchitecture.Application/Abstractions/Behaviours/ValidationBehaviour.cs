using CleanArchitecture.Application.Exceptions;
using FluentValidation;
using MediatR;

namespace CleanArchitecture.Application.Abstractions.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    public async Task<TResponse> Handle(TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
    {
        //Existe alguna validación?
        if (!_validators.Any())
        {
            return await next();
        }

        //Validar el request
        var context = new ValidationContext<TRequest>(request);

        //Retornamos los errores de validación
        var validationErrors =
               _validators.Select(validators => validators.Validate(context))
               .Where(result => !result.Errors.Any())
               .SelectMany(result => result.Errors)
               //Quiero mapear los errores a un string
               .Select(error => new ValidationError(
                      error.PropertyName,
                      error.ErrorMessage
               )).ToList();

        //Si hay errores de validación, lanzamos una excepción
        //No se puede continuar con el siguiente paso
        if (validationErrors.Any())
        {
            throw new ValidationExceptions(validationErrors);
        }

        //si no hay errores de validación, continuamos con el siguiente paso
        return await next();
    }
}