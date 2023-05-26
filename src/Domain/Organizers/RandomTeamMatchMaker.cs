using Domain.Common.Extensions;

namespace Domain.Organizers;
public class RandomTeamMatchMaker : ITeamMatchMaker
{
    public IEnumerable<Match> CreateMatches(Tournament tournament)
    {
        var resultCount = 0;
        var shuffledTeams = tournament.Teams.Shuffle().ToList();
        for (var i = 0; resultCount < CollectionMiddle(shuffledTeams.Count); i += 2)
        {
            if (IsTheEndIndexOfTheCollection(i, shuffledTeams.Count) && !IsEven(shuffledTeams.Count))
            {
                yield return Match.Create(tournament, shuffledTeams[i], default!);
                break;
            }
            yield return Match.Create(tournament, shuffledTeams[i], shuffledTeams[i + 1]);
            resultCount++;
        }
    }

    static double CollectionMiddle(int collectionCount)
        => Math.Ceiling(collectionCount / 2.0);

    static bool IsTheEndIndexOfTheCollection(int idx, int collectionCount)
        => idx == collectionCount - 1;

    static bool IsEven(int collectionCount)
        => collectionCount % 2 == 0;
}