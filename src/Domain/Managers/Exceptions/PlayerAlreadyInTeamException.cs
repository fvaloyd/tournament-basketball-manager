using Domain.Common.Exceptions;

namespace Domain.Managers.Exceptions;
public sealed class PlayerAlreadyInTeamException : BadRequestException
{
    public PlayerAlreadyInTeamException() : base() {}

    public PlayerAlreadyInTeamException(Guid playerId) : base($"Player with id: {playerId} already register on the team.") {}

    public PlayerAlreadyInTeamException(string? message, Exception? innerException) : base(message, innerException) {}

    public PlayerAlreadyInTeamException(string? message) : base(message) {}
}