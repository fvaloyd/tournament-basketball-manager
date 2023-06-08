using Domain.Common;
using Domain.Players;
using Domain.Players.Exceptions;
using Domain.Managers.Exceptions;
using Domain.Managers.DomainEvents;

namespace Domain.Managers;
public sealed class Manager : Entity
{
    public ManagerPersonalInfo PersonalInfo { get; private set; }
    public Guid TeamId { get; private set; }
    public Team? Team { get; private set; }
    public bool AreLeadingATeam => Team is not null;

    private Manager(){}
    private Manager(ManagerPersonalInfo personalInfo)
        => PersonalInfo = personalInfo;

    public static Manager Create(ManagerPersonalInfo personalInfo)
    {
        ManagerPersonalInfoNullException.ThrowIfNull(personalInfo, "Manager personal info could not be null.");
        var manager = new Manager(personalInfo);
        manager.RaiseEvent(new ManagerCreatedDomainEvent(manager.Id));
        return manager;
    }

    public void CreateTeam(string teamName)
    {
        if (Team is not null && TeamId != Guid.Empty)
            throw new ManagerAlreadyHaveATeamException(Id, TeamId);
        if (string.IsNullOrWhiteSpace(teamName))
            throw new InvalidTeamNameException("Invalid team name.");
        var team = Team.Create(teamName, this);
        AssignTeam(team);
    }

    public static Manager Create(ManagerPersonalInfo personalInfo, string teamName)
    {
        var manager = Create(personalInfo);
        var team = Team.Create(teamName, manager);
        manager.AssignTeam(team);
        return manager;
    }

    private void AssignTeam(Team team)
        => (Team, TeamId) = (team, team.Id);

    public void DraftPlayer(Player player)
    {
        PlayerNullException.ThrowIfNull(player);
        if (Team is null)
            throw new ManagerDoesNotHaveATeamException();
        Team.DraftPlayer(player);
    }

    public void ReleasePlayer(Guid playerId)
    {
        if (Team is null)
            throw new ManagerDoesNotHaveATeamException();
        Team.ReleasePlayer(playerId);
    }

    public void DissolveTheTeam()
    {
        if (Team is null && TeamId == Guid.Empty)
            throw new ManagerDoesNotHaveATeamException();
        Team!.ReleaseAllPlayers();
        RaiseEvent(new TeamDissolvedDomainEvent(TeamId, Id));
        Team = default!;
        TeamId = Guid.Empty;
    }
}

public sealed record ManagerPersonalInfo(
    string FirstName,
    string LastName,
    string Email,
    DateTime DateOfBirth,
    string Country,
    string City,
    string Street,
    string HouseNumber,
    string Code
) : PersonalInfo(FirstName: FirstName, LastName: LastName, Email: Email, DateOfBirth: DateOfBirth, Country, City, Street, HouseNumber, Code);