using Shared;
using static Presentation.Web.Pages.Managers.CreateManagerComponent;

namespace Presentation.Web.Pages.Managers;
public class ManagerState
{
    private ManagerResponse _currentManager= default!;
    private List<ManagerResponse> _managers = new();

    public ManagerResponse CurrentManager => _currentManager;
    public IReadOnlyCollection<ManagerResponse> Managers => _managers.AsReadOnly();

    public void SetCurrentManager(Guid id)
        => _currentManager= _managers.First(o => o.Id == id);
    public void AddManager(ManagerResponse manager)
        => _managers.Add(manager);
    public void AddManager(object formModel, Guid id)
    {
        ManagerPersonalInfoFormModel personalInfo = ((CreateManagerForm)formModel).ManagerPersonalInfo;
        ManagerResponse manager = new()
        {
            Id = id,
            PersonalInfo = new Domain.Managers.ManagerPersonalInfo(
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
        AddManager(manager);
    }
    public void InitializeManagers(IEnumerable<ManagerResponse> managers)
        => _managers = managers.ToList();
}