using Domain.Common;

namespace Domain.Organizers.DomainEvents;
public sealed record TournamentCreatedDomainEvent(
    Guid TournamentId,
    Guid OrganizerId
) : DomainEvent(Guid.NewGuid());