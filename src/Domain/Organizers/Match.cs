using Domain.Common;
using Domain.Managers;
using Domain.Organizers.Exceptions;
using Domain.Organizers.DomainEvents;

namespace Domain.Organizers;
public sealed class Match : Entity
{
    public Guid TournamentId { get; private set; }
    public Tournament Tournament { get; private set; }
    public Guid TeamAId { get; private set; }
    public Team TeamA { get; private set; }
    public Guid TeamBId { get; private set; }
    public Team TeamB { get; private set; }

    private Match(Tournament tournament, Team teamA, Team teamB)
    {
        Tournament = tournament;
        TournamentId = tournament.Id;
        TeamA = teamA;
        TeamAId = teamA is default(Team) ? Guid.Empty : teamA.Id;
        TeamB = teamB;
        TeamBId = teamB is default(Team) ? Guid.Empty : teamB.Id;
    }

    public static Match Create(Tournament tournament, Team teamA, Team teamB)
    {
        TournamentNullException.ThrowIfNull(tournament);
        var match = new Match(tournament, teamA, teamB);
        match.RaiseEvent(new MatchCreatedDomainEvent(
            match.Id,
            match.TeamAId,
            match.TeamBId,
            match.TournamentId));
        return match;
    }
}