using Application.Features.Managers.DTOs;

namespace Application.Features.Organizers.DTOs;
public record TournamentResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public IEnumerable<TeamResponse> Teams { get; init; } = null!;
    public Guid OrganizerId { get; init; }
}