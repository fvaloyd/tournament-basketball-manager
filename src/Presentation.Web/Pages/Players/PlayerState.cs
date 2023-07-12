using Shared;
using static Presentation.Web.Pages.Players.CreatePlayerComponent;

namespace Presentation.Web.Pages.Players;

public class PlayerState
{
    private List<PlayerResponse> _players = new();
    private PlayerResponse _currentPlayer;
    public IReadOnlyCollection<PlayerResponse> Players => _players.AsReadOnly();
    public PlayerResponse CurrentPlayer => _currentPlayer;

    public void SetCurrentPlayer(Guid id)
        => _currentPlayer = _players.First(p => p.Id == id);
    public void AddPlayer(PlayerResponse player)
        => _players.Add(player);
    public void AddPlayer(object formModel, Guid id)
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
        AddPlayer(player);
    }
    public void InitializePlayers(IEnumerable<PlayerResponse> players)
        => _players = players.ToList();
}
