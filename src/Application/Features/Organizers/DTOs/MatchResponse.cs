using Application.Features.Managers.DTOs;

namespace Application.Features.Organizers.DTOs;
public record MatchResponse
{
    public Guid TournamentId { get; set; }
    public TeamResponse TeamA { get; set; } = null!;
    public TeamResponse TeamB { get; set; } = null!;
}