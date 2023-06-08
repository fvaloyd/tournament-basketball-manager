using Domain.Common;

namespace Domain.Managers.DomainEvents;

public sealed record ManagerDraftedAPlayerDomainEvent(
    Guid ManagerId,
    Guid PlayerId
) : DomainEvent(Guid.NewGuid());