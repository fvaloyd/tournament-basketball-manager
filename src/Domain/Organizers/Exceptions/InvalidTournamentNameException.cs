using Domain.Common.Exceptions;

namespace Domain.Organizers.Exceptions;
public sealed class InvalidTournamentNameException : BadRequestException
{
    public InvalidTournamentNameException() : base() {}

    public InvalidTournamentNameException(string? message) : base(message) {}

    public InvalidTournamentNameException(string? message, Exception? innerException) : base(message, innerException) {}
}