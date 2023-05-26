using Domain.Common.Extensions;

namespace Domain.UnitTests.Common.Extensions;
public class CollectionExtensionsTests
{
    [Fact]
    public void Shuffle_ShouldReturnAShuffledCollection()
    {
        var collection = Enumerable.Range(1, 100);
        var collectionShuffled = collection;

        collectionShuffled = collectionShuffled.Shuffle();

        collectionShuffled.Should().NotContainInConsecutiveOrder(collection);
    }
}