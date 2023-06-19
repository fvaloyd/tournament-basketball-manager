using Domain.Common;
using Domain.Players;
using Domain.Organizers;
using Domain.Players.Exceptions;
using Domain.Managers.Exceptions;
using Domain.Managers.DomainEvents;
using Domain.Organizers.Exceptions;

namespace Domain.Managers;
public sealed class Team : Entity
{
    public string Name { get; private set; } = string.Empty;
    private readonly HashSet<Player> _players = new();
    public IReadOnlySet<Player> Players => _players;
    public Guid ManagerId { get; private set; }
    public Manager Manager { get; private set; }
    public Guid? TournamentId { get; private set; }
    public Tournament? Tournament { get; private set; }

    #pragma warning disable CS8618
    private Team(){}
    private Team(
        string name,
        Manager manager
    )
    {
        Name = name;
        Manager = manager;
        ManagerId = manager.Id;
    }

    public static Team Create(string name, Manager manager)
    {
        ManagerNullException.ThrowIfNull(manager, "Manager could not be null.");
        var team = new Team(name, manager);
        team.RaiseEvent(new TeamCreatedDomainEvent(team.Id));
        return team;
    }

    public void DraftPlayer(Player player)
    {
        PlayerNullException.ThrowIfNull(player, "Player could not be null.");
        var result = _players.Add(player);
        if (!result)
            throw new PlayerAlreadyInTeamException(player.Id);
        player.JoinATeam(this);
    }

    public void ReleasePlayer(Guid playerId)
    {
        var playerToRelease = _players.FirstOrDefault(p => p.Id == playerId) ?? throw new PlayerNotFoundException(playerId);
        _players.Remove(playerToRelease);
        playerToRelease.LeaveTheTeam();
    }

    public void ReleaseAllPlayers()
    {
        if (!_players.Any())
            return;
        foreach(var player in _players)
        {
            ReleasePlayer(player.Id);
        }
    }

    public void Modifyname(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new InvalidTeamNameException("Invalid name for a team");
        Name = newName;
        RaiseEvent(new TeamChangedItsNameDomainEvent(Id, Name));
    }

    public void RegisterInATournament(Tournament tournament)
    {
        TournamentNullException.ThrowIfNull(tournament, "Tournament could not be null.");
        if (Tournament is not null)
            throw new TeamAlreadyRegisteredInATournamentException(Id, tournament.Id);
        Tournament = tournament;
        TournamentId = tournament.Id;
    }

    public void LeaveTheTournament()
    {
        if (Tournament is null && TournamentId == Guid.Empty)
            return;
        Tournament = default;
        TournamentId = null;
    }
}