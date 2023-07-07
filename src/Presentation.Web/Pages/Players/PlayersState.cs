using Shared;

namespace Presentation.Web.Pages.Players;

public class PlayersState
{
    public List<PlayerResponse> Players { get; set; } = new List<PlayerResponse>();
    public PlayerResponse CurrentPlayer { get; set; } = null!;
}
