using Domain.Common;

namespace Domain.Players.DomainEvents;
public sealed record PlayerLeavedTheTeamDomainEvent(
    Guid PlayerId,
    Guid TeamId
) : DomainEvent(Guid.NewGuid());