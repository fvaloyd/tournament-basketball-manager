using Domain.Common;

namespace Domain.Organizers.DomainEvents;
public sealed record OrganizerCreatedDomainEvent(
    Guid OrganizerId
) : DomainEvent(Guid.NewGuid());