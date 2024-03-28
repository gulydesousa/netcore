using CleanArchitecture.Domain.Abstractions;
using MediatR;

namespace CleanArchitecture.Application.Abstractions.Messaging;

// IQuery is a marker interface for queries
// IQuery<TResponse> is a marker interface for queries that return a response
// IQuery<TResponse> inherits IRequest<Result<TResponse>>
// IRequest<TResponse> is a marker interface for requests
// IRequest<Result<TResponse>> is a marker interface for requests that return a response
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{ }
