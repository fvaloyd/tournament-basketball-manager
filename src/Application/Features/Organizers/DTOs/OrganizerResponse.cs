using Domain.Organizers;

namespace Application.Features.Organizers.DTOs;
public record OrganizerResponse
{
    public Guid Id { get; set; }
    public OrganizerPersonalInfo PersonalInfo { get; set; } = null!;
    public Guid TournamentId { get; set; }
    public TournamentResponse? Tournament { get; set; } = null!;
}