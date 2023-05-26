using Domain.Common;

namespace Domain.UnitTests;
public class ConcreteEntityTest
{
    [Fact]
    public void ConcreteEntities_ShouldBeSealed()
    {
        IEnumerable<Type> concreteEntites = typeof(Entity).Assembly
                                                        .GetTypes()
                                                        .Where(t => t.IsAssignableTo(typeof(Entity)) && !t.IsAbstract);
        foreach (Type concreteEntity in concreteEntites)
        {
            concreteEntity.IsSealed.Should().BeTrue();
        }
    }
}