using Shared;

namespace Presentation.Web.Pages.Organizers;
public class OrganizerState
{
    public OrganizerResponse CurrentOrganizer { get; set; } = null!;
    public List<OrganizerResponse> Organizers { get; set; } = new List<OrganizerResponse>();
}