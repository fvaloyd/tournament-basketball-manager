using Domain.Common;
using Domain.Services;
using Domain.Managers;
using Domain.Organizers;
using Domain.Organizers.Exceptions;
using Domain.Organizers.DomainEvents;

namespace Domain.UnitTests.Organizers;
public class OrganizerTests
{
    [Fact]
    public void IsOrganizingATournament_ShouldBeTrue_WhenOrganizerHaveATournamentAssociated()
    {
        var organizer = Organizer.Create(new("test", "test", "test@gamil.com", DateTime.Now, "", "", "", "", ""));
        organizer.CreateTournament("tt");

        organizer.IsOrganizingATournament.Should().BeTrue();
    }

    [Fact]
    public void IsOrganizingATournament_ShoulBeFalse_WhenTheOrganizerDoesNotHaveATournamentAssociated()
    {
        var organizer = Organizer.Create(new("test", "test", "test@gamil.com", DateTime.Now, "", "", "", "", ""));

        organizer.IsOrganizingATournament.Should().BeFalse();
    }

    [Fact]
    public void Create_ShouldThrowAOrganizerPersonalInfoNullExcpetion_WhenNullOrganizerPersonalInfoIsPassed()
    {
        Action action = () => Organizer.Create(null!);

        action.Should().Throw<OrganizerPersonalInfoNullException>();
    }

    [Fact]
    public void Create_ShouldReturnAnInstanceOfOrganizer_WhenValidOrganizerPersonalInfoIsPassed()
    {
        var validOrganizerPersonalInfo = new OrganizerPersonalInfo("test", "test", "test@gamil.com", DateTime.Now, "", "", "", "", "");

        var organizer = Organizer.Create(validOrganizerPersonalInfo);

        organizer.Should().NotBeNull();
        organizer.GetType().Should().Be(typeof(Organizer));
    }

    [Fact]
    public void Create_ShouldRaiseAOrganizerCreatedDomainEvent_WhenValidOrganizerPersonalInfoIsPassed()
    {
        var validOrganizerPersonalInfo = new OrganizerPersonalInfo("test", "test", "test@gamil.com", DateTime.Now, "", "", "", "", "");

        var organizer = Organizer.Create(validOrganizerPersonalInfo);

        var @event = organizer.DomainEvents.FirstOrDefault(de => de.GetType() == typeof(OrganizerCreatedDomainEvent));
        @event.Should().NotBeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("       ")]
    public void CreateTournament_ShouldThrowAInvalidTournamentNameException_WhenInvalidTournamentNameIsPassed(string invalidName)
    {
        var organizer = Organizer.Create(new("test", "test", "test@gamil.com", DateTime.Now, "", "", "", "", ""));

        Action action = () => organizer.CreateTournament(invalidName);

        action.Should().Throw<InvalidTournamentNameException>();
    }

    [Fact]
    public void CreateTournament_ShouldCreateATournamentAndAddedToTheOrganizer_WhenValidTournamentNameIsPassed()
    {
        const string validTeamName = "valid team name";
        var organizer = Organizer.Create(new("test", "test", "test@gamil.com", DateTime.Now, "", "", "", "", ""));

        organizer.CreateTournament(validTeamName);

        organizer.Tournament!.Name.Should().Be(validTeamName);
    }

    [Fact]
    public void RegisterTeam_ShouldThrowAOrganizerDoesNotHaveTournamentException_WhenTheOrganizerDoesNotHaveATournamentAssociated()
    {
        var organizer = Organizer.Create(new("test", "test", "test@gamil.com", DateTime.Now, "", "", "", "", ""));

        Action action = () => organizer.RegisterTeam(null!);

        action.Should().Throw<OrganizerDoesNotHaveTournamentException>();
    }

    [Fact]
    public void RegisterTeam_ShouldRegisterATeamInTheTournament_WhenTheOrganizerHaveATeamAndValidArgumentArePassed()
    {
        var validTeam = Team.Create("test", Manager.Create(new("test", "test", "test", DateTime.Now, "", "", "", "", "")));
        var organizer = Organizer.Create(new("test", "test", "test@gamil.com", DateTime.Now, "", "", "", "", ""));
        organizer.CreateTournament("tournament");

        organizer.RegisterTeam(validTeam);

        organizer.Tournament!.Teams.Should().Contain(validTeam);
        validTeam.Tournament.Should().Be(organizer.Tournament);
    }

    [Fact]
    public void DiscardTeam_ShouldThrowAOrganizerDoesNotHaveTournament_WhenTheOrganizerDoesNotHaveATournamentAssociated()
    {
        var organizer = Organizer.Create(new("test", "test", "test@gamil.com", DateTime.Now, "", "", "", "", ""));

        Action action = () => organizer.DiscardTeam(Guid.Empty);

        action.Should().Throw<OrganizerDoesNotHaveTournamentException>();
    }

    [Fact]
    public void DiscardTeam_ShouldDiscardTheTeamFromTheTournament_WhenTheOrganizerHaveATournamentAndValidArgumentArePassed()
    {
        var validTeam = Team.Create("test", Manager.Create(new("test", "test", "test", DateTime.Now, "", "", "", "", "")));
        var organizer = Organizer.Create(new("test", "test", "test@gamil.com", DateTime.Now, "", "", "", "", ""));
        organizer.CreateTournament("tournament");
        organizer.RegisterTeam(validTeam);

        organizer.DiscardTeam(validTeam.Id);

        organizer.Tournament!.Teams.Should().NotContain(validTeam);
    }

    [Fact]
    public void MatchTeams_ShouldThrowAnOrganizerDoesNotHaveTournamentException_WhenTheOrganizerDoesNotHaveATournament()
    {
        var organizer = Organizer.Create(new("test", "test", "test@gamil.com", DateTime.Now, "", "", "", "", ""));

        Action action = () => organizer.MatchTeams(new RandomTeamMatchMaker());

        action.Should().Throw<OrganizerDoesNotHaveTournamentException>();
    }

    [Fact]
    public void MatchTeams_ShouldMatchTheTeams_WhenTheTeamsAreNotPairedYet()
    {
        var organizer = GetOrganizerWithTournamentAndTeams();

        organizer.MatchTeams(new RandomTeamMatchMaker());

        organizer.Tournament!.Matches.Should().NotBeNull();
        organizer.Tournament.Matches.Should().NotBeEmpty();
    }

    [Fact]
    public void GetTournamentMatches_ShouldThrowAOrganizerDoesNotHaveTournament_WhenTheOrganizerDoesNotHaveATournamentAssociated()
    {
        var organizer = Organizer.Create(new("test", "test", "test@gamil.com", DateTime.Now, "", "", "", "", ""));

        Action action = () => organizer.GetTournamentMatches();

        action.Should().Throw<OrganizerDoesNotHaveTournamentException>();
    }

    [Fact]
    public void GetTournamentMatches_ShouldGetTheTournamentMatches_WhenTheOrganizerHaveATournamentAndValidArgumentArePassed()
    {
        var organizer = GetOrganizerWithTournamentAndTeams();
        organizer.MatchTeams(new RandomTeamMatchMaker());

        var matches = organizer.GetTournamentMatches();

        matches.Should().NotBeNull();
        matches.Should().HaveCountGreaterThanOrEqualTo(1);
    }

    [Fact]
    public void FinishTournament_ShouldThrowAOrganizerDoesNotHaveTournament_WhenTheOrganizerDoesNotHaveATournamentAssociated()
    {
        var organizer = Organizer.Create(new("test", "test", "test@gamil.com", DateTime.Now, "", "", "", "", ""));

        Action action = () => organizer.FinishTournament();

        action.Should().Throw<OrganizerDoesNotHaveTournamentException>();
    }

    [Fact]
    public void FinishTournament_ShouldDiscardAllTeamsInTheTournamentAndDropTheTournament_WhenTheOrganizerIsAssociatedWithATournament()
    {
        var organizer = GetOrganizerWithTournamentAndTeams();
        var tournament = organizer.Tournament;
        var teams = organizer.Tournament!.Teams;

        organizer.FinishTournament();

        tournament!.Teams.Should().BeNullOrEmpty();
        foreach (var team in teams)
        {
            team.Tournament.Should().BeNull();
            team.TournamentId.Should().Be(Guid.Empty);
        }
    }

    [Fact]
    public void FinishTournament_ShouldRaiseATournamentFinishedDomainEvent_WhenFinishTournamentWasSuccess()
    {
        var organizer = GetOrganizerWithTournamentAndTeams();

        organizer.FinishTournament();

        var @event = organizer.DomainEvents.FirstOrDefault(de => de.GetType() == typeof(TournamentFinishedDomainEvent));
        @event.Should().NotBeNull();
    }

    private static Organizer GetOrganizerWithTournamentAndTeams()
    {
        var t1 = Team.Create("test", Manager.Create(new("test1", "test", "test", DateTime.Now, "", "", "", "", "")));
        var t2 = Team.Create("test", Manager.Create(new("test2", "test", "test", DateTime.Now, "", "", "", "", "")));
        var organizer = Organizer.Create(new("test", "test", "test@gamil.com", DateTime.Now, "", "", "", "", ""));
        organizer.CreateTournament("test tournament");
        organizer.RegisterTeam(t1);
        organizer.RegisterTeam(t2);
        return organizer;
    }
}