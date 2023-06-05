using Domain.Common;
using Domain.Services;
using Domain.Managers;
using Domain.Organizers;

namespace Domain.UnitTests.Organizers;
public class RandomTeamMatchMakerTests
{
    [Fact]
    public void CreateMatches_ShouldReturnACollectionOfMatchesWithTheHalfOfElementsThatTheTeamsCollectionInTheTournamentHas_WhenValidArgumentIsPassed()
    {
        var maker = new RandomTeamMatchMaker();
        var tournament = GetTournamentWithSpecifiedTeams(10);

        var matches =  maker.CreateMatches(tournament);

        matches.Count().Should().Be((int)Math.Ceiling(tournament.Teams.Count / 2.0));
    }

    [Fact]
    public void CreateMatches_ShouldInitializeAMatchPropertyTeamBWithADefaultTeam_WhenNoTeamAvailableToMatch()
    {
        var maker = new RandomTeamMatchMaker();
        var tournament = GetTournamentWithSpecifiedTeams(11);

        var matches =  maker.CreateMatches(tournament).ToList();

        matches.Last().TeamB.Should().Be(default(Team));
    }

    private static Tournament GetTournamentWithSpecifiedTeams(int numbOfTeams)
    {
        List<Team> teams = new();
        for (var i = 1; i <= numbOfTeams; i++)
        {
            teams.Add(Team.Create($"team:{i}", Manager.Create(new($"organizer:{i}", "test", "test", DateTime.Now, new("test", "test", "test", "test", "test")))));
        }
        var organizer = Organizer.Create(new ("test", "test", "test@gamil.com", DateTime.Now, new Address("", "", "", "", "")));
        organizer.CreateTournament("test tournament");
        var tournament = organizer.Tournament;

        foreach (var team in teams)
        {
            tournament!.RegisterTeam(team);
        }

        return tournament!;
    }
}