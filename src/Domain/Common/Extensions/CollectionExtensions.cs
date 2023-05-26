namespace Domain.Common.Extensions;
public static class CollectionExtensions
{
    private readonly static Random _rnd = new();
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        => source.OrderBy(_ => _rnd.Next());
}