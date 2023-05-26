using Domain.Common;
using Domain.Managers;
using Domain.Organizers.Exceptions;
using Domain.Organizers.DomainEvents;

namespace Domain.Organizers;
public sealed class Organizer : Entity
{
    public OrganizerPersonalInfo PersonalInfo { get; private set; }
    public Guid TournamentId { get; private set; }
    public Tournament? Tournament { get; private set; }
    public bool IsOrganizingATournament => Tournament is not null;

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
        if (Tournament is null && TournamentId == Guid.Empty)
            throw new OrganizerDoesNotHaveTournamentException();
        Tournament!.RegisterTeam(team);
    }

    public void DiscardTeam(Guid teamId)
    {
        if (Tournament is null && TournamentId == Guid.Empty)
            throw new OrganizerDoesNotHaveTournamentException();
        Tournament!.DiscardTeam(teamId);
    }

    public void FinishTournament()
    {
        if (Tournament is null && TournamentId == Guid.Empty)
            throw new OrganizerDoesNotHaveTournamentException();
        Tournament!.ReleaseAllTeams();
        RaiseEvent(new TournamentFinishedDomainEvent(TournamentId, Id));
        Tournament = default!;
        TournamentId = Guid.Empty;
    }

    public IEnumerable<Match> GetTournamentMatches(ITeamMatchMaker teamMatchMaker)
    {
        if (Tournament is null && TournamentId == Guid.Empty)
            throw new OrganizerDoesNotHaveTournamentException();
        if (Tournament!.Matches.Any())
            return Tournament.Matches;
        Tournament.Match(teamMatchMaker);
        return Tournament.Matches;
    }
}

public sealed record OrganizerPersonalInfo(
    string FirstName,
    string LastName,
    string Email,
    DateTime DateOfBirht,
    Address Address
) : PersonalInfo(FirstName: FirstName, LastName: LastName, Email: Email, DateOfBirht: DateOfBirht, Address: Address);