using Shared;

namespace Presentation.Web.Pages.Players;

public class PlayersState
{
    public List<PlayerResponse> Players { get; set; }
    public PlayerResponse CurrentPlayer { get; set; }
}
