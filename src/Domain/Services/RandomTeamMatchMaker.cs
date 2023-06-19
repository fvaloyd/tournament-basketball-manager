using Domain.Organizers;
using Domain.Common.Extensions;
using Domain.Organizers.Exceptions;

namespace Domain.Services;
public class RandomTeamMatchMaker : ITeamMatchMaker
{
    public IEnumerable<Match> CreateMatches(Tournament tournament)
    {
        if (tournament.Teams.Count % 2 != 0)
            throw new NumberOfTeamIsNotEvenException($"The numbers of teams are: {tournament.Teams.Count}, to match the teams you need to discard a team or register another one.");
        
        var resultCount = 0;
        var shuffledTeams = tournament.Teams.Shuffle().ToList();
        
        for (var i = 0; resultCount < CollectionMiddle(shuffledTeams.Count); i += 2)
        {
            yield return Match.Create(tournament, shuffledTeams[i], shuffledTeams[i + 1]);
            resultCount++;
        }
    }

    static double CollectionMiddle(int collectionCount)
        => Math.Ceiling(collectionCount / 2.0);
}