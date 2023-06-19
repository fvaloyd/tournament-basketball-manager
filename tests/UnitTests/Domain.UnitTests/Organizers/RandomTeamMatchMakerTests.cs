using Domain.Services;
using Domain.Managers;
using Domain.Organizers;
using Domain.Organizers.Exceptions;

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
    public void CreateMatches_ShouldThrowAnException_WhenTheNumbersOfTeamsIsNotEven()
    {
        var maker = new RandomTeamMatchMaker();
        var tounament = GetTournamentWithSpecifiedTeams(9);
        Action act = () => maker.CreateMatches(tounament).ToList();
        act.Should().Throw<NumberOfTeamIsNotEvenException>();
    }

    private static Tournament GetTournamentWithSpecifiedTeams(int numbOfTeams)
    {
        List<Team> teams = new();
        for (var i = 1; i <= numbOfTeams; i++)
        {
            teams.Add(Team.Create($"team:{i}", Manager.Create(new($"organizer:{i}", "test", "test", DateTime.Now, "", "", "", "", ""))));
        }
        var organizer = Organizer.Create(new ("test", "test", "test@gamil.com", DateTime.Now, "", "", "", "", ""));
        organizer.CreateTournament("test tournament");
        var tournament = organizer.Tournament;

        foreach (var team in teams)
        {
            tournament!.RegisterTeam(team);
        }

        return tournament!;
    }
}