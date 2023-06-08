using Domain.Common;
using Domain.Players;
using Domain.Managers;
using Domain.Players.Exceptions;
using Domain.Players.DomainEvents;

namespace Domain.UnitTests.Players;
public class PlayerTests
{
    [Fact]
    public void IsInTeam_ShouldBeTrue_WhenPlayerIsInATeam()
    {
        var team = Team.Create("Team test", Manager.Create(new ManagerPersonalInfo("test", "test", "test@gmail.com", DateTime.Now, "test", "test", "test", "test", "test")));
        var player = Player.Create(new("test", "test", "test@test.com", DateTime.Now, 1.80f, 80.5f, "RD", "SJO", "S", "57", "93000"), Position.PointGuard);
        player.JoinATeam(team);

        player.IsInTeam.Should().BeTrue();
    }

    [Fact]
    public void IsInTeam_ShouldBeFalse_WhenPlayerIsNotInATeam()
    {
        var player = Player.Create(new("test", "test", "test@test.com", DateTime.Now, 1.80f, 80.5f, "RD", "SJO", "S", "57", "93000"), Position.PointGuard);

        player.IsInTeam.Should().BeFalse();
    }

    [Fact]
    public void Create_ShouldCreateAPlayerInstance()
    {
        PlayerPersonalInfo playerPersonalInfo = new("test", "test", "test@test.com", DateTime.Now, 1.80f, 80.5f, "RD", "SJO", "S", "57", "93000");
        const Position position = Position.PointGuard;

        var player = Player.Create(playerPersonalInfo, position);

        player.GetType().Should().Be(typeof(Player));
        player.PersonalInfo.Should().Be(playerPersonalInfo);
        player.Position.Should().Be(position);
    }

    [Fact]
    public void Create_ShouldThrowAnPlayerPersonalInfoNullException_WhenPlayerPerosnalInfoIsNull()
    {
        const Position position = Position.PointGuard;

        Action action = () => Player.Create(null!, position);

        action.Should().Throw<PlayerPersonalInfoNullException>();
    }

    [Fact]
    public void Create_ShouldRaiseAPlayerCreatedEvent()
    {
        PlayerPersonalInfo playerPersonalInfo = new("test", "test", "test@test.com", DateTime.Now, 1.80f, 80.5f, "RD", "SJO", "S", "57", "93000");

        var player = Player.Create(playerPersonalInfo, Position.PointGuard);

        player.DomainEvents.Should().NotBeEmpty();
        player.DomainEvents[0].Should().BeOfType<PlayerCreatedDomainEvent>();
    }

    [Fact]
    public void JoinATeam_ShouldAssignATeamToThePlayer_WhenThePlayerIsNotCurrentlyOnATeam()
    {
        var team = Team.Create("Team test", Manager.Create(new ManagerPersonalInfo("test", "test", "test@gmail.com", DateTime.Now, "test", "test", "test", "test", "test")));
        PlayerPersonalInfo playerPersonalInfo = new("test", "test", "test@test.com", DateTime.Now, 1.80f, 80.5f, "RD", "SJO", "S", "57", "93000");
        var player = Player.Create(playerPersonalInfo, Position.PointGuard);

        player.JoinATeam(team);

        player.Team.Should().NotBeNull();
        player.Team.Should().Be(team);
        player.TeamId.Should().Be(team.Id);
    }

    [Fact]
    public void JoinATeam_ShouldThrowAPlayerAlreadyInATeamException_WhenThePlayerAlreadyAreInATeam()
    {
        var team = Team.Create("Team test", Manager.Create(new ManagerPersonalInfo("test", "test", "test@gmail.com", DateTime.Now, "test", "test", "test", "test", "test")));
        PlayerPersonalInfo playerPersonalInfo = new("test", "test", "test@test.com", DateTime.Now, 1.80f, 80.5f, "RD", "SJO", "S", "57", "93000");
        var player = Player.Create(playerPersonalInfo, Position.PointGuard);
        player.JoinATeam(team);

        Action action = () => player.JoinATeam(null!);

        action.Should().Throw<PlayerAlreadyInATeamException>();
    }

    // [Fact]
    // public void JoinATeam_ShouldRaiseAPlayerJoinedATeamDomainEvent()
    // {
    //     var team = Team.Create("Team test", Manager.Create(new ManagerPersonalInfo("test", "test", "test@gmail.com", DateTime.Now, "test", "test", "test", "test", "test")));
    //     PlayerPersonalInfo playerPersonalInfo = new("test", "test", "test@test.com", DateTime.Now, 1.80f, 80.5f, "RD", "SJO", "S", "57", "93000");
    //     var player = Player.Create(playerPersonalInfo, Position.PointGuard);

    //     player.JoinATeam(team);

    //     player.DomainEvents.Should().NotBeEmpty();
    //     var @event = player.DomainEvents.FirstOrDefault(de => de.GetType() == typeof(PlayerJoinedATeamDomainEvent));
    //     @event.Should().NotBeNull();
    // }

    [Fact]
    public void LeaveTheTeam_ShouldClearTheTeamAndTeamIdPorperties()
    {
        var team = Team.Create("Team test", Manager.Create(new ManagerPersonalInfo("test", "test", "test@gmail.com", DateTime.Now, "test", "test", "test", "test", "test")));
        PlayerPersonalInfo playerPersonalInfo = new("test", "test", "test@test.com", DateTime.Now, 1.80f, 80.5f, "RD", "SJO", "S", "57", "93000");
        var player = Player.Create(playerPersonalInfo, Position.PointGuard);
        player.JoinATeam(team);

        player.LeaveTheTeam();

        player.Team.Should().BeNull();
        player.TeamId.Should().Be(Guid.Empty);
    }

    // [Fact]
    // public void LeaveTheTeam_ShouldRaiseAPlayerLeavedTheTeamDomainEvent_WhenThePlayerBelongsToATeam()
    // {
    //     var team = Team.Create("Team test", Manager.Create(new ManagerPersonalInfo("test", "test", "test@gmail.com", DateTime.Now, "test", "test", "test", "test", "test")));
    //     PlayerPersonalInfo playerPersonalInfo = new("test", "test", "test@test.com", DateTime.Now, 1.80f, 80.5f, "RD", "SJO", "S", "57", "93000");
    //     var player = Player.Create(playerPersonalInfo, Position.PointGuard);
    //     player.JoinATeam(team);

    //     player.LeaveTheTeam();

    //     var @event = player.DomainEvents.FirstOrDefault(de => de.GetType() == typeof(PlayerLeavedTheTeamDomainEvent));
    //     @event.Should().NotBeNull();
    // }

    [Fact]
    public void LeaveTheTeam_ShouldNotRaiseAPlayerLeavedTheTeamDomainEvent_WhenThePlayerDoesNotBelongsToATeam()
    {
        PlayerPersonalInfo playerPersonalInfo = new("test", "test", "test@test.com", DateTime.Now, 1.80f, 80.5f, "RD", "SJO", "S", "57", "93000");
        var player = Player.Create(playerPersonalInfo, Position.PointGuard);

        player.LeaveTheTeam();

        var @event = player.DomainEvents.FirstOrDefault(de => de.GetType() == typeof(PlayerLeavedTheTeamDomainEvent));
        @event.Should().BeNull();
    }
}