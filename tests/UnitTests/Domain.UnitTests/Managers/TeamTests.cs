using Domain.Common;
using Domain.Players;
using Domain.Managers;
using Domain.Organizers;
using Domain.Players.Exceptions;
using Domain.Managers.Exceptions;
using Domain.Managers.DomainEvents;
using Domain.Organizers.Exceptions;

namespace Domain.UnitTests.Managers;
public class TeamTests
{
    [Fact]
    public void Create_ShouldCreateAnInstanceOfTeam()
    {
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "", "", "", "", ""));

        var team = Team.Create("test", manager);

        team.GetType().Should().Be(typeof(Team));
        team.Manager.Should().Be(manager);
        team.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Create_ShouldThrowAnManagerNullException_WhenNullManagerIsPassed()
    {
        Action action = () => Team.Create("test", null!);

        action.Should().Throw<ManagerNullException>();
    }

    [Fact]
    public void Create_ShouldRaiseATeamCreatedDomainEvent_WhenTheCreationIsSuccess()
    {
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "", "", "", "", ""));

        var team = Team.Create("test", manager);

        var @event = team.DomainEvents.FirstOrDefault(de => de.GetType() == typeof(TeamCreatedDomainEvent));
        @event.Should().NotBeNull();
    }

    [Fact]
    public void DraftPlayer_ShouldThrowAnPlayerNullException_WhenNullPlayerIsPassed()
    {
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "", "", "", "", ""));
        var team = Team.Create("test", manager);

        Action action = () => team.DraftPlayer(null!);

        action.Should().Throw<PlayerNullException>();
    }

    [Fact]
    public void DraftPlayer_ShouldAddThePlayerToTheTeam_WhenTheTeamDoesNotHaveThatPlayerOnItsRoster()
    {
        var player = Player.Create(new("test", "test", "test@test.com", DateTime.Now, 1.80f, 80.5f, "RD", "SJO", "S", "57", "93000"), Position.PointGuard);
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "", "", "", "", ""));
        var team = Team.Create("test", manager);

        team.DraftPlayer(player);

        team.Players.First().Should().Be(player);
    }

    [Fact]
    public void DraftPlayer_ShouldReturnAPlayerAlreadyInTeamException_WhenPlayerAlreadyAreInTheRoster()
    {
        var player = Player.Create(new("test", "test", "test@test.com", DateTime.Now, 1.80f, 80.5f, "RD", "SJO", "S", "57", "93000"), Position.PointGuard);
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "", "", "", "", ""));
        var team = Team.Create("test", manager);
        team.DraftPlayer(player);

        Action action = () => team.DraftPlayer(player);

        action.Should().Throw<PlayerAlreadyInTeamException>();
    }

    [Fact]
    public void ReleasePlayer_ShouldThrowAPlayerNotFoundException_WhenThePlayerWasNotFound()
    {
        var player = Player.Create(new("test", "test", "test@test.com", DateTime.Now, 1.80f, 80.5f, "RD", "SJO", "S", "57", "93000"), Position.PointGuard);
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "", "", "", "", ""));
        var team = Team.Create("test", manager);
        team.DraftPlayer(player);

        Action action = () => team.ReleasePlayer(Guid.Empty);

        action.Should().Throw<PlayerNotFoundException>();
    }

    [Fact]
    public void ReleasePlayer_ShouldReleaseThePlayer_WhenThePlayerIsFound()
    {
        var player = Player.Create(new("test", "test", "test@test.com", DateTime.Now, 1.80f, 80.5f, "RD", "SJO", "S", "57", "93000"), Position.PointGuard);
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "", "", "", "", ""));
        var team = Team.Create("test", manager);
        team.DraftPlayer(player);

        team.ReleasePlayer(player.Id);

        var result = team.Players.FirstOrDefault(p => p.Id == player.Id);
        result.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("         ")]
    public void ModifyName_ShouldThrowAnInvalidTeamNameException_WhenInvalidNameIsPassed(string invalidName)
    {
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "", "", "", "", ""));
        var team = Team.Create("test", manager);

        Action action = () => team.Modifyname(invalidName);

        action.Should().Throw<InvalidTeamNameException>();
    }

    [Fact]
    public void ModifyName_ShouldModifyTheName_WhenValidNameIsPassed()
    {
        const string newName = "newName";
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "", "", "", "", ""));
        var team = Team.Create("test", manager);

        team.Modifyname(newName);

        team.Name.Should().Be(newName);
    }

    [Fact]
    public void ModifyName_ShouldRaiseATeamChangedItsNameDomainEvent()
    {
        const string newName = "newName";
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "", "", "", "", ""));
        var team = Team.Create("test", manager);

        team.Modifyname(newName);

        var @event = team.DomainEvents.FirstOrDefault(de => de.GetType() == typeof(TeamChangedItsNameDomainEvent));
        @event.Should().NotBeNull();
    }

    [Fact]
    public void RegisterInATournament_ShouldThrowAnTournamentNullExcpetion_WhenTournamentPassedIsNull()
    {
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "", "", "", "", ""));
        var team = Team.Create("test", manager);

        Action action = () => team.RegisterInATournament(null!);

        action.Should().Throw<TournamentNullException>();
    }

    [Fact]
    public void RegisterInATournament_ShouldRegisterTheTeamInTheTournament_WhenTheTeamIsNotInATournamentAndAValidTournamentIsPassed()
    {
        var tournament = Tournament.Create("test", Organizer.Create(new("test", "test", "test", DateTime.Now, "", "", "", "", "")));
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "", "", "", "", ""));
        var team = Team.Create("test", manager);

        team.RegisterInATournament(tournament);

        team.Tournament.Should().Be(tournament);
        team.TournamentId.Should().Be(tournament.Id);
    }

    [Fact]
    public void RegisterInATournament_ShouldRaiseATeamRegisteredInATournamentDomainEvent_WhenTheRegistrationSucceed()
    {
        var tournament = Tournament.Create("test", Organizer.Create(new("test", "test", "test", DateTime.Now, "", "", "", "", "")));
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "", "", "", "", ""));
        var team = Team.Create("test", manager);

        team.RegisterInATournament(tournament);

        var @event = team.DomainEvents.FirstOrDefault(de => de.GetType() == typeof(TeamRegisteredInATournamentDomainEvent));
        @event.Should().NotBeNull();
    }

    [Fact]
    public void RegisterInTournament_ShouldThrowATeamAlreadyRegisteredInATournamentException_WhenTheTeamAlreadyRegisteredInATournament()
    {
        var tournament = Tournament.Create("test", Organizer.Create(new("test", "test", "test", DateTime.Now, "", "", "", "", "")));
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "", "", "", "", ""));
        var team = Team.Create("test", manager);
        team.RegisterInATournament(tournament);

        Action action = () => team.RegisterInATournament(tournament);

        action.Should().Throw<TeamAlreadyRegisteredInATournamentException>();
    }

    [Fact]
    public void LeaveTheTournament_ShouldCleanTheTournamentAndTournamentIdProperties_WhenTheTeamIsInATournament()
    {
        var tournament = Tournament.Create("test", Organizer.Create(new("test", "test", "test", DateTime.Now, "", "", "", "", "")));
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "", "", "", "", ""));
        var team = Team.Create("test", manager);
        team.RegisterInATournament(tournament);

        team.LeaveTheTournament();

        team.Tournament.Should().BeNull();
        team.TournamentId.Should().Be(Guid.Empty);
    }

    [Fact]
    public void LeaveTheTournament_ShouldRaiseATeamAbandonedTheTournamentDomainEvent_WhenTheTeamIsInATournament()
    {
        var tournament = Tournament.Create("test", Organizer.Create(new("test", "test", "test", DateTime.Now, "", "", "", "", "")));
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "", "", "", "", ""));
        var team = Team.Create("test", manager);
        team.RegisterInATournament(tournament);

        team.LeaveTheTournament();

        var @event = team.DomainEvents.FirstOrDefault(de => de.GetType() == typeof(TeamAbandonedTheTournamentDomainEvent));
        @event.Should().NotBeNull();
    }

    [Fact]
    public void LeaveTheTournament_ShouldDoNothing_WhenATeamIsNotInATournamentYet()
    {
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "", "", "", "", ""));
        var team = Team.Create("test", manager);

        team.LeaveTheTournament();

        var @event = team.DomainEvents.FirstOrDefault(de => de.GetType() == typeof(TeamAbandonedTheTournamentDomainEvent));
        @event.Should().BeNull();
    }

    [Fact]
    public void ReleaseAllPlayers_ShouldReleaseAllPlayerInTheTeam()
    {
        var p1 = Player.Create(new("test1", "test", "test@test.com", DateTime.Now, 1.80f, 80.5f, "RD", "SJO", "S", "57", "93000"), Position.PointGuard);
        var p2 = Player.Create(new("test2", "test", "test@test.com", DateTime.Now, 1.80f, 80.5f, "RD", "SJO", "S", "57", "93000"), Position.PointGuard);
        var team = Team.Create("myteam", Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "", "", "", "", "")));
        team.DraftPlayer(p1);
        team.DraftPlayer(p2);

        team.ReleaseAllPlayers();

        p1.Team.Should().BeNull();
        p1.TeamId.Should().Be(Guid.Empty);
        p2.Team.Should().BeNull();
        p2.TeamId.Should().Be(Guid.Empty);
    }
}