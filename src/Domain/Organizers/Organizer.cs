using Domain.Common;
using Domain.Services;
using Domain.Managers;
using Domain.Organizers.Exceptions;
using Domain.Organizers.DomainEvents;
using Domain.Managers.DomainEvents;

namespace Domain.Organizers;
public sealed class Organizer : Entity
{
    public OrganizerPersonalInfo PersonalInfo { get; private set; }
    public Guid? TournamentId { get; private set; }
    public Tournament? Tournament { get; private set; }
    public bool IsOrganizingATournament => Tournament is not null;

    #pragma warning disable CS8618
    public Organizer(){}
    private Organizer(OrganizerPersonalInfo personalInfo)
        => PersonalInfo = personalInfo;

    public static Organizer Create(OrganizerPersonalInfo personalInfo)
    {
        OrganizerPersonalInfoNullException.ThrowIfNull(personalInfo, "Organizer personal info could not be null");
        var organizer = new Organizer(personalInfo);
        organizer.RaiseEvent(new OrganizerCreatedDomainEvent(organizer.Id));
        return organizer;
    }

    public void CreateTournament(string tournamentName)
    {
        var tournament = Tournament.Create(tournamentName, this);
        Tournament = tournament;
        TournamentId = tournament.Id;
    }

    public void RegisterTeam(Team team)
    {
        if (Tournament is null && TournamentId == Guid.Empty || TournamentId == null)
            throw new OrganizerDoesNotHaveTournamentException();
        Tournament!.RegisterTeam(team);
        RaiseEvent(new TeamRegisteredInATournamentDomainEvent(Id, TournamentId));
    }

    public void DiscardTeam(Guid teamId)
    {
        if (Tournament is null && TournamentId == Guid.Empty || TournamentId == null)
            throw new OrganizerDoesNotHaveTournamentException();
        Tournament!.DiscardTeam(teamId);
        RaiseEvent(new TeamEliminatedFromTheTournamentDomainEvent(teamId, TournamentId));
    }

    public void FinishTournament()
    {
        if (Tournament is null && TournamentId == Guid.Empty || TournamentId == null)
            throw new OrganizerDoesNotHaveTournamentException();
        Tournament!.ReleaseAllTeams();
        RaiseEvent(new TournamentFinishedDomainEvent(TournamentId, Id));
        Tournament = default!;
        TournamentId = Guid.Empty;
    }

    public void MatchTeams(ITeamMatchMaker teamMatchMaker)
    {
        if (Tournament is null && TournamentId == Guid.Empty || TournamentId == null)
            throw new OrganizerDoesNotHaveTournamentException();
        if (Tournament!.Matches.Any())
            return;
        Tournament!.Match(teamMatchMaker);
    }

    public IEnumerable<Match> GetTournamentMatches()
    {
        if (Tournament is null && TournamentId == Guid.Empty || TournamentId == null)
            throw new OrganizerDoesNotHaveTournamentException();
        if (!Tournament!.Matches.Any())
            throw new TeamsAreNotPairedYetException();
        return Tournament.Matches;
    }
}

public sealed record OrganizerPersonalInfo(
    string FirstName,
    string LastName,
    string Email,
    DateTime DateOfBirth,
    string Country,
    string City,
    string Street,
    string HouseNumber,
    string Code
) : PersonalInfo(FirstName: FirstName, LastName: LastName, Email: Email, DateOfBirth: DateOfBirth, Country, City, Street, HouseNumber, Code);