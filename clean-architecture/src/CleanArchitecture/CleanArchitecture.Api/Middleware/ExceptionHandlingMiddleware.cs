using CleanArchitecture.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next; 
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next, 
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Call the next middleware in the pipeline
            //Si no sucede nada, se ejecuta el siguiente middleware
            //Continua con el contexto
            await _next(context);
        }
        catch (Exception ex)
        {
            //Captura de los errores en la aplicacion
            //1. Escribir en el log
            _logger.LogError(ex, "Ocurrio un error en la aplicacion: {Message}", ex.Message);
            
            //2. Recupera los detalles de la excepcion
            //Segun el tipo de excepcion, se obtienen los detalles
            //Se envia un objeto con los detalles de la excepcion
            var exceptionDetails = GetExceptionDetails(ex);

            //3. Escribir en el contexto de la respuesta e un objeto de tipo ProblemDetails
            var problemDetails = new ProblemDetails
            {
                Status = exceptionDetails.Status,
                Type = exceptionDetails.Type,
                Title = exceptionDetails.Title,
                Detail = exceptionDetails.Detail,
            };
            //Trasladamos los errors en el objeto de respuesta            
            if(exceptionDetails is not null)
            {
                problemDetails.Extensions["errors"] = exceptionDetails.Errors;
            }
            
            //4. Escribir en el contexto de la respuesta
            context.Response.StatusCode = exceptionDetails!.Status;
            
            //Escribir en el contexto de la respuesta en formato JSON
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }

    private static ExceptionDetails GetExceptionDetails(Exception exception)
    {
        return exception switch
        {
            ValidationExceptions validationException => new ExceptionDetails(
                StatusCodes.Status400BadRequest,
                "ValidationFailure",
                "Validación de error.",
                "Han ocurrido uno o mas errores de validacion en la solicitud.",
                validationException.Errors
            ),
            _ => new ExceptionDetails(
                StatusCodes.Status500InternalServerError,
                "Error",
                "Error interno del servidor.",
                "Ocurrio un error interno en la aplicacion.",
                null
            )
        };
    }

    internal record ExceptionDetails(
        int Status,
        string Type,
        string Title,
        string Detail,
        IEnumerable<object>? Errors 
    );
}