using Domain;
using Domain.Players;

namespace Shared;
public record PlayerResponse : IEntity
{
    public Guid Id { get; init; }
    public PlayerPersonalInfo PersonalInfo { get; init; } = null!;
    public Position Position { get; init; }
    public Guid TeamId { get; set; }
}