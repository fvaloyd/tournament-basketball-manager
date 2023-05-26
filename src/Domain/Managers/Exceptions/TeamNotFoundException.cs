using Domain.Common.Exceptions;

namespace Domain.Managers.Exceptions;
public sealed class ManagerDoesNotHaveATeamException : NotFoundException
{
    public ManagerDoesNotHaveATeamException() : base("Manager does not have a team associated yet.") {}

    public ManagerDoesNotHaveATeamException(string? message) : base(message) {}

    public ManagerDoesNotHaveATeamException(string? message, Exception? innerException) : base(message, innerException) {}
}