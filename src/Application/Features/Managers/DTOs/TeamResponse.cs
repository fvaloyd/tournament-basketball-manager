using Application.Features.Players;

namespace Application.Features.Managers.DTOs;
public record TeamResponse
{
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public IEnumerable<PlayerResponse> Players { get; init; } = null!;
    public Guid TournamentId { get; init; }
}