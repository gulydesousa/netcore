using CleanArchitecture.Domain.Abstractions;
using MediatR;

namespace CleanArchitecture.Application.Abstractions.Messaging;

/// <summary>
/// Represents a command handler that processes a command.
/// Que diferenica hay entre ICommand y ICommandHandler?
/// ICommand es un comando que se envía a través de la aplicación.
/// ICommandHandler es un manejador de comandos que procesa un comando.
/// ICommandHandler es una interfaz de MediatR que representa un manejador de comandos.
/// Se usa para definir un manejador de comandos.
/// </summary>
public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
where TCommand : ICommand
{}

/// <summary>
/// Represents a command handler that processes a command and returns a response.
/// ICommandHandler<TCommand, TResponse> es una interfaz de MediatR 
/// que representa un manejador de comandos.
/// Se usa para definir un manejador de comandos que procesa un comando y devuelve una respuesta.
/// Recibe un comando y devuelve un resultado.
/// Implementa la interfaz IRequestHandler<TCommand, Result<TResponse>> de MediatR.
/// IRequestHandler<TCommand, Result<TResponse>> es una interfaz de MediatR que 
/// representa un manejador de solicitudes. Recibe una solicitud y devuelve un resultado.
/// </summary>
public interface ICommandHandler<TCommand, TResponse> 
: IRequestHandler<TCommand, Result<TResponse>>
where TCommand : ICommand<TResponse>
{}

