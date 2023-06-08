using Domain.Common;

namespace Domain.Organizers.DomainEvents;

public sealed record TeamEliminatedFromTheTournamentDomainEvent(
    Guid TeamId,
    Guid TournamentId
) : DomainEvent(Guid.NewGuid());