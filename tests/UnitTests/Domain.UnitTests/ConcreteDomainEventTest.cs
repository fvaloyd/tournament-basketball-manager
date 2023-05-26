using Domain.Common;

namespace Domain.UnitTests;
public class ConcreteDomainEventTest
{
    [Fact]
    public void BaseDomainEventClass_ShouldBeAsbtract()
    {
        typeof(DomainEvent).IsAbstract.Should().BeTrue();
    }

    [Fact]
    public void ConcreteDomainEvent_ShouldBeSealedClasses()
    {
        IEnumerable<Type> concreteDomainEvents = typeof(DomainEvent).Assembly
                                                            .GetTypes()
                                                            .Where(t => t.IsAssignableTo(typeof(DomainEvent)) && !t.IsAbstract);
        foreach (var domainEvent in concreteDomainEvents)
        {
            domainEvent.IsSealed.Should().BeTrue();
            domainEvent.IsClass.Should().BeTrue();
        }
    }
}