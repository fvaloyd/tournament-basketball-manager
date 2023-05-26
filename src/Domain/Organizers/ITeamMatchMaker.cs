namespace Domain.Organizers;
public interface ITeamMatchMaker
{
    IEnumerable<Match> CreateMatches(Tournament tournament);
}