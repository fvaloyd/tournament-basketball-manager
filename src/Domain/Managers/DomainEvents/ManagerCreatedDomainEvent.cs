using Domain.Common;

namespace Domain.Managers.DomainEvents;
public sealed record ManagerCreatedDomainEvent(
    Guid ManagerId
) : DomainEvent(Guid.NewGuid());