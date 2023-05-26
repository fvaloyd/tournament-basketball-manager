using Domain.Common.Exceptions;

namespace Domain.Players.Exceptions;
public sealed class PlayerAlreadyInATeamException : BadRequestException
{
    public PlayerAlreadyInATeamException() : base() {}

    public PlayerAlreadyInATeamException(string? message) : base(message) {}

    public PlayerAlreadyInATeamException(Guid playerId, Guid teamId) : base($"Player with id: {playerId} is already allied to a team with id: {teamId}") {}

    public PlayerAlreadyInATeamException(string? message, Exception? innerException) : base(message, innerException) {}
}