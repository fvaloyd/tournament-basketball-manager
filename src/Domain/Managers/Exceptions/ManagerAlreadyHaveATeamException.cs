using Domain.Common.Exceptions;

namespace Domain.Managers.Exceptions;
public sealed class ManagerAlreadyHaveATeamException : BadRequestException
{
    public ManagerAlreadyHaveATeamException() : base() {}

    public ManagerAlreadyHaveATeamException(string? message) : base(message) {}

    public ManagerAlreadyHaveATeamException(Guid managerId, Guid? teamId) : base($"Manager {managerId} already have a team with id of {teamId}.") {}

    public ManagerAlreadyHaveATeamException(string? message, Exception? innerException) : base(message, innerException) {}
}