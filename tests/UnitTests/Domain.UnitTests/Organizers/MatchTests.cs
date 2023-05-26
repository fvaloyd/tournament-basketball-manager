using Domain.Managers;
using Domain.Organizers;
using Domain.Organizers.Exceptions;
using Domain.Organizers.DomainEvents;

namespace Domain.UnitTests.Organizers;
public class MatchTests
{
    [Fact]
    public void Create_ShouldThrowAnTournamentNullException_WhenTournamentIsNull()
    {
        Action actionWithTournamentNull = () => Match.Create(null!, Team.Create("", Manager.Create(new("", "", "", DateTime.Now, new("", "", "", "", "")))), Team.Create("", Manager.Create(new("", "", "", DateTime.Now, new("", "", "", "", "")))));

        actionWithTournamentNull.Should().Throw<TournamentNullException>();
    }

    [Fact]
    public void Create_ShouldReturnAnInstanceOfMatch_WhenValidArgumentArePassed()
    {
        var (tournament, teamA, teamB) = GetValidArgumentForCreateAMatch();

        var match = Match.Create(tournament, teamA, teamB);

        match.Should().NotBeNull();
        match.Tournament.Should().Be(tournament);
        match.TournamentId.Should().Be(tournament.Id);
        match.TeamA.Should().Be(teamA);
        match.TeamAId.Should().Be(teamA.Id);
        match.TeamB.Should().Be(teamB);
        match.TeamBId.Should().Be(teamB.Id);
    }

    [Fact]
    public void Create_ShouldRaiseAMatchCreatedDomainEvent_WhenValidArgumentArePassed()
    {
        var (tournament, teamA, teamB) = GetValidArgumentForCreateAMatch();

        var match = Match.Create(tournament, teamA, teamB);

        var @event = match.DomainEvents.FirstOrDefault(de => de.GetType() == typeof(MatchCreatedDomainEvent));
        @event.Should().NotBeNull();
    }

    public static (Tournament tournament, Team teamA, Team teamB) GetValidArgumentForCreateAMatch()
    {
        var teamA = Team.Create("teamA", Manager.Create(new("", "", "", DateTime.Now, new("", "", "", "", ""))));
        var teamB = Team.Create("teamB", Manager.Create(new("", "", "", DateTime.Now, new("", "", "", "", ""))));
        var tournament = Tournament.Create("tournament", Organizer.Create(new("", "", "", DateTime.Now, new("", "", "", "", ""))));

        return (tournament, teamA, teamB);
    }
}