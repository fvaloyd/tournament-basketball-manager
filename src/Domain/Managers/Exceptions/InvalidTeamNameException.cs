using Domain.Common.Exceptions;

namespace Domain.Managers.Exceptions;
public sealed class InvalidTeamNameException : BadRequestException
{
    public InvalidTeamNameException() : base() {}

    public InvalidTeamNameException(string? message) : base(message) {}

    public InvalidTeamNameException(string? message, Exception? innerException) : base(message, innerException) {}
}