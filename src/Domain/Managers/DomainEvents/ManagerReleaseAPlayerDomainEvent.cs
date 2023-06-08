using Domain.Common;

namespace Domain.Managers.DomainEvents;

public sealed record ManagerReleaseAPlayerDomainEvent(
    Guid ManagerId,
    Guid PlayerId
) : DomainEvent(Guid.NewGuid());