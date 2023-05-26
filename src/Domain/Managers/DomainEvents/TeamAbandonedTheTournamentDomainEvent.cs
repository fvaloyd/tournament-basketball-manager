using Domain.Common;

namespace Domain.Managers.DomainEvents;
public sealed record TeamAbandonedTheTournamentDomainEvent(
    Guid TeamId,
    Guid TournamentId
) : DomainEvent(Guid.NewGuid());