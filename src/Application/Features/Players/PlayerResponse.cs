using Domain.Players;

namespace Application.Features.Players;
public record PlayerResponse
{
    public Guid Id { get; init; }
    public PlayerPersonalInfo PlayerPersonalInfo { get; init; } = null!;
    public Guid TeamId { get; set; }
}