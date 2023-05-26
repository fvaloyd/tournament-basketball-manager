using Domain.Common;

namespace Domain.Managers.DomainEvents;
public sealed record TeamChangedItsNameDomainEvent(
    Guid TeamId,
    string NewName
) : DomainEvent(Guid.NewGuid());