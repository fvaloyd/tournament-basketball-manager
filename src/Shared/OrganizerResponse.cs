using Domain;
using Domain.Organizers;

namespace Shared;
public record OrganizerResponse : IEntity
{
    public Guid Id { get; set; }
    public OrganizerPersonalInfo PersonalInfo { get; set; } = null!;
    public Guid TournamentId { get; set; }
    public TournamentResponse? Tournament { get; set; } = null!;
}