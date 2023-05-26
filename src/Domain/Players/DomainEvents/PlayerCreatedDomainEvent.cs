using Domain.Common;

namespace Domain.Players.DomainEvents;
public sealed record PlayerCreatedDomainEvent(Guid PlayerId) : DomainEvent(Guid.NewGuid());