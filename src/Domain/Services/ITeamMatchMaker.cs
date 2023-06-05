using Domain.Organizers;

namespace Domain.Services;
public interface ITeamMatchMaker
{
    IEnumerable<Match> CreateMatches(Tournament tournament);
}