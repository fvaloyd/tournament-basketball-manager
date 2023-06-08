using Domain.Common;
using Domain.Services;
using Domain.Managers;
using Domain.Managers.Exceptions;
using Domain.Organizers.Exceptions;
using Domain.Organizers.DomainEvents;

namespace Domain.Organizers;
public sealed class Tournament : Entity
{
    public string Name { get; private set; } = string.Empty;
    public Guid OrganizerId { get; private set; }
    public Organizer Organizer { get; private set; } = default!;
    private readonly HashSet<Team> _teams = new();
    public IReadOnlySet<Team> Teams => _teams;
    private readonly HashSet<Match> _matches = new();
    public IReadOnlySet<Match> Matches => _matches;

    private Tournament(){}
    private Tournament(string name, Organizer organizer) : base()
    {
        Name = name;
        Organizer = organizer;
        OrganizerId = organizer.Id;
    }

    public static Tournament Create(string name, Organizer creator)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidTournamentNameException("Invalid tournament name.");
        OrganizerNullException.ThrowIfNull(creator, "Organizer could not be null.");
        var tournament = new Tournament(name, creator);
        tournament.RaiseEvent(new TournamentCreatedDomainEvent(tournament.Id, tournament.OrganizerId));
        return tournament;
    }

    public void RegisterTeam(Team team)
    {
        TeamNullException.ThrowIfNull(team);
        var result = _teams.Add(team);
        if (!result)
            throw new TeamAlreadyInTournamentException(team.Id);
        team.RegisterInATournament(this);
    }

    public void DiscardTeam(Guid teamId)
    {
        var teamToDiscard = _teams.FirstOrDefault(t => t.Id == teamId) ?? throw new TeamNotFoundException(teamId);
        _teams.Remove(teamToDiscard);
        teamToDiscard.LeaveTheTournament();
    }

    public void ReleaseAllTeams()
    {
        if (!_teams.Any())
            return;
        foreach (var team in _teams)
        {
            DiscardTeam(team.Id);
        }
    }

    public void Match(ITeamMatchMaker teamMatchMaker)
    {
        if (_matches.Any())
            throw new TeamsAreAlreadyPairedException(Id);
        foreach (var match in teamMatchMaker.CreateMatches(this))
        {
            _matches.Add(match);
        }
    }
}