using Domain.Common;

namespace Domain.Managers.DomainEvents;
public sealed record TeamRegisteredInATournamentDomainEvent(
    Guid TeamId,
    Guid? TournamentId
) : DomainEvent(Guid.NewGuid());