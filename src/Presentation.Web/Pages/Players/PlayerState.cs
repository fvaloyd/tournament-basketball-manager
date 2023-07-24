using Presentation.Web.Common;
using Shared;
using static Presentation.Web.Pages.Players.CreatePlayerComponent;

namespace Presentation.Web.Pages.Players;

public class PlayerState : EntityBaseState<PlayerResponse>
{
    public override void AddEntity(object formModel, Guid id)
    {
        PlayerPersonalInfoFormModel personalInfo = ((CreatePlayerForm)formModel).PlayerPersonalInfo;
        PlayerResponse player = new()
        {
            Id = id,
            PersonalInfo = new(
                personalInfo.FirstName,
                personalInfo.LastName,
                personalInfo.Email,
                personalInfo.DateOfBirth ?? DateTime.Now,
                personalInfo.Height,
                personalInfo.Weight,
                personalInfo.Country,
                personalInfo.City,
                personalInfo.Street,
                personalInfo.HouseNumber,
                personalInfo.Code)
        };
        Add(player);
    }
}
