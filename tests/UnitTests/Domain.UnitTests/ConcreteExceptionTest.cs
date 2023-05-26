using Domain.Common.Exceptions;

namespace Domain.UnitTests;
public class ConcreteExceptionTest
{
    [Fact]
    public void NotFoundExceptionBase_ShouldBeAbstract()
    {
        typeof(NotFoundException).IsAbstract.Should().BeTrue();
    }

    [Fact]
    public void BadRequestExceptionBase_ShouldBeAbstract()
    {
        typeof(BadRequestException).IsAbstract.Should().BeTrue();
    }

    [Fact]
    public void ConcreteNotFoundExceptions_ShouldBeSealed()
    {
        foreach (Type concreteNotFoundExceptionType in GetConcreteExceptionTypes(typeof(NotFoundException)))
        {
            concreteNotFoundExceptionType.IsSealed.Should().BeTrue();
        }
    }

    [Fact]
    public void ConcreteBadRequestExceptions_ShouldBeSealed()
    {
        foreach (Type concreteBadRequestExceptionType in GetConcreteExceptionTypes(typeof(BadRequestException)))
        {
            concreteBadRequestExceptionType.IsSealed.Should().BeTrue();
        }
    }

    private static IEnumerable<Type> GetConcreteExceptionTypes(Type type)
        => type.Assembly
                .GetTypes()
                .Where(t => t.IsAssignableTo(type) && !t.IsAbstract);
}