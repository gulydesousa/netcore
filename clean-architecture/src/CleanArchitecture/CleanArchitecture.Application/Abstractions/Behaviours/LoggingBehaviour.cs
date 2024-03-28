using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Abstractions.Behaviours;
public class LoggingBehaviour<TRequest, TResponse>
: IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    //Capturará todos los request de tipo commnad que se envíen desde el cliente
    //Cuando se pida insertar un vehiculo, se capturará el request y se logueará
    //Esto solo evaluará los request de tipo command
    //NO evaluará los request de tipo query
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var name = request.GetType().Name;

        try
        {
            _logger.LogInformation("CleanArchitecture Request: {name}", name);
            var result = await next();
            _logger.LogInformation("Comando {name} ejecutado exitosamente", name);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en la ejecución del comando {name}", name);
            throw;
        }
    }
}
