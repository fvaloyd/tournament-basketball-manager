using Domain.Common;

namespace Domain.Organizers.DomainEvents;
public sealed record MatchCreatedDomainEvent(
    Guid MatchId,
    Guid TeamAId,
    Guid TeamBId,
    Guid TournamentId
) : DomainEvent(Guid.NewGuid());