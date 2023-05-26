using Domain.Common;

namespace Domain.UnitTests.Common;
public class EntityTests
{
    [Fact]
    public void Entity_ShouldBeAsbtract()
    {
        typeof(Entity).IsAbstract.Should().BeTrue();
    }

    [Fact]
    public void RaiseEvent_ShouldAddADomainEvent()
    {
        var entity = new TestEntity();

        entity.RaiseEvent(new TestEvent(Guid.NewGuid()));

        entity.DomainEvents.Should().NotBeEmpty();
        entity.DomainEvents.Count.Should().Be(1);
    }
    [Fact]
    public void ClearEvents_ShouldClearAllEvents()
    {
        var entity = new TestEntity();
        entity.RaiseEvent(new TestEvent(Guid.NewGuid()));
        entity.RaiseEvent(new TestEvent(Guid.NewGuid()));
        entity.RaiseEvent(new TestEvent(Guid.NewGuid()));

        entity.ClearEvents();

        entity.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_ShouldGenerateAGuidIdIfNoOneIsProvide()
    {
        var entity = new TestEntity();

        entity.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Constructor_ShouldAssignTheGuidIdProvidedInTheConstructor()
    {
        var id = Guid.NewGuid();

        var entity = new TestEntity(id);

        entity.Id.Should().Be(id);
    }
}

public record TestEvent(Guid Id) : DomainEvent(Id);

public class TestEntity : Entity
{
    public TestEntity(Guid id) : base(id) { }
    public TestEntity() { }
}