using Domain.Common;

namespace Domain.Managers.DomainEvents;
public sealed record TeamDissolvedDomainEvent(
    Guid? TeamId,
    Guid ManagerId
) : DomainEvent(Guid.NewGuid());