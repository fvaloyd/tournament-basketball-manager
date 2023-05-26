using Domain.Common;

namespace Domain.Players.DomainEvents;
public sealed record PlayerJoinedATeamDomainEvent(
    Guid PlayerId,
    Guid TeamId
) : DomainEvent(Guid.NewGuid());