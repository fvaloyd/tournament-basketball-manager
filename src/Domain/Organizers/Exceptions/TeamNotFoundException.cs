using Domain.Common.Exceptions;

namespace Domain.Organizers.Exceptions;
public sealed class TeamNotFoundException : NotFoundException
{
    public TeamNotFoundException() : base() {}

    public TeamNotFoundException(string? message) : base(message) {}

    public TeamNotFoundException(Guid? teamId) : base($"Team with id: {teamId} was not found") {}

    public TeamNotFoundException(string? message, Exception? innerException) : base(message, innerException) {}
}