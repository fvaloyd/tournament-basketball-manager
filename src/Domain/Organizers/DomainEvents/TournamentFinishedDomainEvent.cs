using Domain.Common;

namespace Domain.Organizers.DomainEvents;
public sealed record TournamentFinishedDomainEvent(
    Guid TournamentId,
    Guid OrganizerId
) : DomainEvent(Guid.NewGuid());