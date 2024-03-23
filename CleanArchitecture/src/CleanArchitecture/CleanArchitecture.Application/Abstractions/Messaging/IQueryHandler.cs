using CleanArchitecture.Domain.Abstractions;
using MediatR;

namespace CleanArchitecture.Application.Abstractions.Messaging;


/// <summary>
/// Represents a query that returns a response.
/// </summary>
/// <typeparam name="IQuery"></typeparam>
/// <typeparam name="TResponse"></typeparam>
//IQueryHandler es una interfaz que se encarga de manejar las consultas que se realizan en la aplicación.

//IQueryHandler<TQuery, TResponse> significa que se encarga de manejar 
//las consultas que se realizan en la aplicación y que devuelven un resultado de tipo TResponse.

//: IRequestHandler<TQuery, Result<TResponse>> 
//significa que IRequestHandler es una interfaz que se encarga de manejar las solicitudes que se realizan en la aplicación 
//y que devuelven un resultado de tipo Result<TResponse>.


public interface IQueryHandler<TQuery, TResponse> 
: IRequestHandler<TQuery, Result<TResponse>>
where TQuery : IQuery<TResponse>
{
    
}