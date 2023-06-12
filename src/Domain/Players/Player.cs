using Domain.Common;
using Domain.Managers;
using Domain.Players.Exceptions;
using Domain.Players.DomainEvents;

namespace Domain.Players;
public sealed class Player : Entity
{
    public PlayerPersonalInfo PersonalInfo { get; private set; }
    public Position Position { get; private set; }
    public Guid TeamId { get; private set; }
    public Team? Team { get; private set; }
    public bool IsInTeam => Team is not null;

    #pragma warning disable CS8618
    public Player(){}
    private Player(
        PlayerPersonalInfo personalInfo,
        Position position) : base()
    {
        PersonalInfo = personalInfo;
        Position = position;
    }

    public static Player Create(PlayerPersonalInfo personalInfo, Position position)
    {
        PlayerPersonalInfoNullException.ThrowIfNull(personalInfo, "Player personal info could not be null.");
        var player = new Player(personalInfo, position);
        player.RaiseEvent(new PlayerCreatedDomainEvent(player.Id));
        return player;
    }

    public void JoinATeam(Team team)
    {
        if (Team is not null)
            throw new PlayerAlreadyInATeamException(Id, TeamId);
        TeamId = team.Id;
        Team = team;
    }

    public void LeaveTheTeam()
    {
        if (Team is null)
            return;
        TeamId = Guid.Empty;
        Team = default;
    }
}