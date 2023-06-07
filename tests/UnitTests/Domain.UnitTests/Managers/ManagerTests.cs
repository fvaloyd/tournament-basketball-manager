using Domain.Common;
using Domain.Players;
using Domain.Managers;
using Domain.Players.Exceptions;
using Domain.Managers.Exceptions;
using Domain.Managers.DomainEvents;

namespace Domain.UnitTests.Managers;
public class ManagerTests
{
    [Fact]
    public void AreLeadingATeam_ShouldBeTrue_WhenManagerHaveATeamAssociated()
    {
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "test", "test", "test", "test", "test"));
        manager.CreateTeam("tt");

        manager.AreLeadingATeam.Should().BeTrue();
    }

    [Fact]
    public void AreLeadingATeam_ShouldBeFalse_WhenManagerDoesNotHaveATeamAssociated()
    {
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "test", "test", "test", "test", "test"));

        manager.AreLeadingATeam.Should().BeFalse();
    }

    [Fact]
    public void Create_ShouldReturnAManagerInstance()
    {
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "test", "test", "test", "test", "test"));

        manager.Should().NotBeNull();
        manager.GetType().Should().Be(typeof(Manager));
    }

    [Fact]
    public void Create_ShouldThrowAnManagerPersonalInfoNullException_WhenNullManagerPersonalInfoIsPassed()
    {
        var action = () => Manager.Create(null!);

        action.Should().Throw<ManagerPersonalInfoNullException>();
    }

    [Fact]
    public void Create_ShouldRaiseAManagerCreatedDomainEvent_WhenValidArgumentIsPassed()
    {
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "test", "test", "test", "test", "test"), "teamtest");

        var @event = manager.DomainEvents.FirstOrDefault(de => de.GetType() == typeof(ManagerCreatedDomainEvent));
        @event.Should().NotBeNull();
    }

    [Fact]
    public void CreateOverLoad_ShouldCreateAManagerAndAssignATeamToIt()
    {
        const string teamName = "test team name";

        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "test", "test", "test", "test", "test"), teamName);

        manager.Team.Should().NotBeNull();
        manager.Team!.Name.Should().Be(teamName);
        manager.TeamId.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void DraftPlayer_ShouldThrowAException_WhenNullPlayerIsPassed()
    {
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "test", "test", "test", "test", "test"));

        Action action = () => manager.DraftPlayer(null!);

        action.Should().Throw<PlayerNullException>();
    }

    [Fact]
    public void DraftPlayer_ShouldThrowAManagerDoesNotHaveATeamException_WhenTheManagerDoesNotHaveAnAssignedTeam()
    {
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "test", "test", "test", "test", "test"));

        Action action = () => manager.DraftPlayer(Player.Create(new("", "", "", DateTime.Now, 7f, 7f, "test", "test", "test", "test", "test"), Position.PointGuard));

        action.Should().Throw<ManagerDoesNotHaveATeamException>();
    }

    [Fact]
    public void ReleasePlayer_ShouldThrowAManagerDoesNotHaveATeamException_WhenTheManagerDoesNotHaveAnAssignedTeam()
    {
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "test", "test", "test", "test", "test"));

        Action action = () => manager.ReleasePlayer(Guid.NewGuid());

        action.Should().Throw<ManagerDoesNotHaveATeamException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("        ")]
    public void CreateTeam_ShouldThrowAInvalidTeamNameException_WhenInvalidTeamNameIsPassed(string invalidTeamName)
    {
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "test", "test", "test", "test", "test"));

        Action action = () => manager.CreateTeam(invalidTeamName);

        action.Should().Throw<InvalidTeamNameException>();
    }

    [Fact]
    public void CreateTeam_ShouldThrowAManagerAlreadyHaveATeamException_WhenManagerAlreadyHaveATeam()
    {
        var managerWithTeam = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "test", "test", "test", "test", "test"), "team");

        Action action = () => managerWithTeam.CreateTeam("team");

        action.Should().Throw<ManagerAlreadyHaveATeamException>();
    }

    [Fact]
    public void CreateTeam_ShouldCreateAndAssignATeamToTheManager_WhenTheManagerDoesntHaveATeamAndAValidTeamNameIsPassed()
    {
        const string validTeamName = "myteam";
        var manager = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "test", "test", "test", "test", "test"));

        manager.CreateTeam(validTeamName);

        manager.Team.Should().NotBeNull();
        manager.Team!.Name.Should().Be(validTeamName);
    }

    [Fact]
    public void DissolveTheTeam_ShouldThrowATeamDoesNotHaveATeamException_WhenTheManagerDoesntHaveATeam()
    {
        var managerWithOutTeam = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "test", "test", "test", "test", "test"));

        Action action = () => managerWithOutTeam.DissolveTheTeam();

        action.Should().Throw<ManagerDoesNotHaveATeamException>();
    }

    [Fact]
    public void DissolveTheTeam_ShouldRemoveTheTeamAndBreakTheRelationWithThePlayers()
    {
        var p1 = Player.Create(new("player1", "test", "player1@gmail.com", DateTime.Now, 6.1f, 80.5f, "test", "test", "test", "test", "test"), Position.PointGuard);
        var p2 = Player.Create(new("player2", "test", "player2@gmail.com", DateTime.Now, 6.6f, 90.5f, "test", "test", "test", "test", "test"), Position.ShootingGuard);
        var managerWithTeam = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "test", "test", "test", "test", "test"), "teamName");
        managerWithTeam.DraftPlayer(p1);
        managerWithTeam.DraftPlayer(p2);

        managerWithTeam.DissolveTheTeam();

        managerWithTeam.Team.Should().BeNull();
        managerWithTeam.TeamId.Should().Be(Guid.Empty);
        p1.Team.Should().BeNull();
        p1.TeamId.Should().Be(Guid.Empty);
        p2.Team.Should().BeNull();
        p2.TeamId.Should().Be(Guid.Empty);
    }

    [Fact]
    public void DissolveTheTeam_ShouldRaiseATeamDissolvedDomainEvent_WhenDissolveTheTeamWasSuccess()
    {
        var p1 = Player.Create(new("player1", "test", "player1@gmail.com", DateTime.Now, 6.1f, 80.5f, "test", "test", "test", "test", "test"), Position.PointGuard);
        var p2 = Player.Create(new("player2", "test", "player2@gmail.com", DateTime.Now, 6.6f, 90.5f, "test", "test", "test", "test", "test"), Position.ShootingGuard);
        var managerWithTeam = Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "test", "test", "test", "test", "test"), "teamName");
        managerWithTeam.DraftPlayer(p1);
        managerWithTeam.DraftPlayer(p2);

        managerWithTeam.DissolveTheTeam();

        var @event = managerWithTeam.DomainEvents.FirstOrDefault(de => de.GetType() == typeof(TeamDissolvedDomainEvent));
        @event.Should().NotBeNull();
    }
}