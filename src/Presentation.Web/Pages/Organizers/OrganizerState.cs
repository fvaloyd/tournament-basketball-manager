using Domain.Organizers;
using Presentation.Web.Common;
using Shared;
using static Presentation.Web.Pages.Organizers.CreateOrganizerComponent;

namespace Presentation.Web.Pages.Organizers;
public class OrganizerState : EntityBaseState<OrganizerResponse>
{
    //private OrganizerResponse _currentOrganizer = default!;
    //private List<OrganizerResponse> _organizers = new();

    //public OrganizerResponse CurrentOrganizer => _currentOrganizer;
    //public IReadOnlyCollection<OrganizerResponse> Organizers => _organizers.AsReadOnly();

    //public void SetCurrentOrganizer(Guid id)
    //    => _currentOrganizer = _organizers.First(o => o.Id == id);
    //public void AddOrganizer(OrganizerResponse organizer)
    //    => _organizers.Add(organizer);
    public override void AddEntity(object formModel, Guid id)
    {
        OrganizerPersonalInfoFormModel personalInfo = ((CreateOrganizerForm)formModel).OrganizerPersonalInfo;
        OrganizerResponse organizer = new()
        {
            Id = id,
            PersonalInfo = new OrganizerPersonalInfo(
                personalInfo.FirstName,
                personalInfo.LastName,
                personalInfo.Email,
                personalInfo.DateOfBirth ?? DateTime.Now,
                personalInfo.Country,
                personalInfo.City,
                personalInfo.Street,
                personalInfo.HouseNumber,
                personalInfo.Code)
        };
        Add(organizer);
    }
    //public void InitializeOrganizers(IEnumerable<OrganizerResponse> organizers)
    //    => _organizers = organizers.ToList();
}