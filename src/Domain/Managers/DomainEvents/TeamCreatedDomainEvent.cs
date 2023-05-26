using Domain.Common;

namespace Domain.Managers.DomainEvents;
public sealed record TeamCreatedDomainEvent(
    Guid TeamId
) : DomainEvent(Guid.NewGuid());