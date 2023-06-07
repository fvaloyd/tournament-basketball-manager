using Domain.Common;
using Domain.Services;
using Domain.Managers;
using Domain.Organizers;
using Domain.Managers.Exceptions;
using Domain.Organizers.Exceptions;
using Domain.Organizers.DomainEvents;

namespace Domain.UnitTests.Organizers;
public class TournamentTests
{
    [Fact]
    public void Create_ShouldReturnAnInstanceOfTournamentWithTheSpecifiedOrganizer()
    {
        var organizer = Organizer.Create(new("", "", "", DateTime.Now, "", "", "", "", ""));

        var tournament = Tournament.Create("test", organizer);

        tournament.Should().NotBeNull();
        tournament.Organizer.Should().Be(organizer);
        tournament.OrganizerId.Should().Be(organizer.Id);
    }

    [Fact]
    public void Create_ShouldThrowAOrganizerNullException_WhenNullOrganizerIsPassed()
    {
        Action action = () => Tournament.Create("test", null!);

        action.Should().Throw<OrganizerNullException>();
    }

    [Fact]
    public void Create_ShouldRaiseATournamentCreatedDomainEvent_WhenCreateTheTournamentWasSuccess()
    {
        var organizer = Organizer.Create(new("", "", "", DateTime.Now, "", "", "", "", ""));

        var tournament = Tournament.Create("test", organizer);

        var @event = tournament.DomainEvents.FirstOrDefault(de => de.GetType() == typeof(TournamentCreatedDomainEvent));
        @event.Should().NotBeNull();
    }

    [Fact]
    public void RegisterTeam_ShouldThrowATeamNullException_WhenNullTeamIsPassed()
    {
        var tournament = Tournament.Create("test", Organizer.Create(new("", "", "", DateTime.Now, "", "", "", "", "")));

        Action action = () => tournament.RegisterTeam(null!);

        action.Should().Throw<TeamNullException>();
    }

    [Fact]
    public void RegisterTeam_ShouldAddTheTeamToTheTournament_WhenValidTeamIsPassed()
    {
        var team = Team.Create("test", Manager.Create(new("", "", "", DateTime.Now, "", "", "", "", "")));
        var tournament = Tournament.Create("test", Organizer.Create(new("", "", "", DateTime.Now, "", "", "", "", "")));

        tournament.RegisterTeam(team);

        tournament.Teams.Should().Contain(team);
    }

    [Fact]
    public void RegisterTeam_ShouldThrowATeamAlreadyInTournamentException_WhenTeamAlReadyInTheTournament()
    {
        var team = Team.Create("test", Manager.Create(new("", "", "", DateTime.Now, "", "", "", "", "")));
        var tournament = Tournament.Create("test", Organizer.Create(new("", "", "", DateTime.Now, "", "", "", "", "")));
        tournament.RegisterTeam(team);

        Action action = () => tournament.RegisterTeam(team);

        action.Should().Throw<TeamAlreadyInTournamentException>();
    }

    [Fact]
    public void DiscardTeam_ShouldThrowATeamNotFoundException_WhenInvalidTeamIdIsPassed()
    {
        var team = Team.Create("test", Manager.Create(new("", "", "", DateTime.Now, "", "", "", "", "")));
        var tournament = Tournament.Create("test", Organizer.Create(new("", "", "", DateTime.Now, "", "", "", "", "")));
        tournament.RegisterTeam(team);

        Action action = () => tournament.DiscardTeam(Guid.NewGuid());

        action.Should().Throw<TeamNotFoundException>();
    }

    [Fact]
    public void DiscardTeam_ShouldDiscardATeam_WhenAValidTeamIdIsPassed()
    {
        var team = Team.Create("test", Manager.Create(new("", "", "", DateTime.Now, "", "", "", "", "")));
        var tournament = Tournament.Create("test", Organizer.Create(new("", "", "", DateTime.Now, "", "", "", "", "")));
        tournament.RegisterTeam(team);

        tournament.DiscardTeam(team.Id);

        tournament.Teams.Should().NotContain(team);
    }

    [Fact]
    public void DiscardTeam_ShouldClearTheRelationBetweenTheTeamAndTournament_WhenAValidTeamIdIsPassed()
    {
        var team = Team.Create("test", Manager.Create(new("", "", "", DateTime.Now, "", "", "", "", "")));
        var tournament = Tournament.Create("test", Organizer.Create(new("", "", "", DateTime.Now, "", "", "", "", "")));
        tournament.RegisterTeam(team);

        tournament.DiscardTeam(team.Id);

        team.Tournament.Should().BeNull();
    }

    [Fact]
    public void ReleaseAllTeams_ShouldDiscardAllTeamsInTheTournament_WhenTheTournamentContainsTeams()
    {
        var t1 = Team.Create("test", Manager.Create(new("", "", "", DateTime.Now, "", "", "", "", "")));
        var t2 = Team.Create("test", Manager.Create(new("", "", "", DateTime.Now, "", "", "", "", "")));
        var tournament = Tournament.Create("test", Organizer.Create(new("", "", "", DateTime.Now, "", "", "", "", "")));
        tournament.RegisterTeam(t1);
        tournament.RegisterTeam(t2);

        tournament.ReleaseAllTeams();

        t1.Tournament.Should().BeNull();
        t1.TournamentId.Should().Be(Guid.Empty);
        t2.Tournament.Should().BeNull();
        t2.TournamentId.Should().Be(Guid.Empty);
        tournament.Teams.Should().BeNullOrEmpty();
    }

    [Fact]
    public void Match_ShoudlMatchTheTeams_WhenTheyAreNotMatches()
    {
        var teams = GetTeamCollection().ToList();
        var tournament = Tournament.Create("test", Organizer.Create(new("", "", "", DateTime.Now, "", "", "", "", "")));
        RegisterTeamCollection(tournament, teams);

        tournament.Match(new RandomTeamMatchMaker());

        tournament.Matches.Count.Should().BeGreaterThanOrEqualTo((int)Math.Ceiling(teams.Count / 2.0));
    }

    [Fact]
    public void Match_ShouldThrowATeamsAreAlreadyPairedException_WhenTeamsAlreadyPaired()
    {
        var teams = GetTeamCollection().ToList();
        var tournament = Tournament.Create("test", Organizer.Create(new("", "", "", DateTime.Now, "", "", "", "", "")));
        RegisterTeamCollection(tournament, teams);
        tournament.Match(new RandomTeamMatchMaker());

        Action action = () => tournament.Match(new RandomTeamMatchMaker());

        action.Should().Throw<TeamsAreAlreadyPairedException>();
    }

    private static void RegisterTeamCollection(Tournament tournament, IEnumerable<Team> teamCollection)
    {
        foreach (var team in teamCollection)
        {
            tournament.RegisterTeam(team);
        }
    }

    private static IEnumerable<Team> GetTeamCollection()
    {
        for (var i = 1; i <= 10; i++)
        {
            yield return Team.Create($"Team Name: {i}", Manager.Create(new("", "", "", DateTime.Now, "", "", "", "", "")));
        }
    }
}