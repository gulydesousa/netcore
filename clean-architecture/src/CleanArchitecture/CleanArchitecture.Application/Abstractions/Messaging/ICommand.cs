using CleanArchitecture.Domain.Abstractions;
using MediatR;

namespace CleanArchitecture.Application.Abstractions.Messaging;

/// <summary>
/// Represents a command that is sent through the application.
/// Esto quiere decir que representa un comando que se envía a través de la aplicación.
/// Un comando es una solicitud de realizar una acción.
/// Por ejemplo, un comando puede ser reservar un alquiler, crear un usuario, etc.
/// Se implementa la interfaz IRequest<Result> que es una interfaz de MediatR.
/// IRequest<TResponse> es una interfaz de MediatR que representa una solicitud de un comando.
/// MediaTR es una biblioteca de .NET que permite implementar el patrón Mediator.
/// El patrón Mediator es un patrón de diseño de software que define 
/// un objeto que encapsula cómo se comunican otros objetos.
/// En este caso, MediatR se usa para implementar el patrón Mediator en la aplicación.
/// Se implementa la interfaz IBaseCommand que es una interfaz personalizada.
/// IBaseCommand es una interfaz personalizada que se usa para marcar una interfaz como un comando.
/// Se usa para definir una interfaz base para los comandos.
/// Se usa para marcar una interfaz como un comando.
/// Se usa para definir una interfaz base para los comandos.
/// </summary>

public interface ICommand : IRequest<Result>, IBaseCommand
{ }

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
{}

public interface IBaseCommand 
{}