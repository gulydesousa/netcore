using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Users.Events;

//sealed: no puede ser heredada
public sealed record UserCreatedDomainEvent(Guid userId) : IDomainEvent;