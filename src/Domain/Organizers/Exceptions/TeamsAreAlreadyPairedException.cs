using Domain.Common.Exceptions;

namespace Domain.Organizers.Exceptions;
public sealed class TeamsAreAlreadyPairedException : BadRequestException
{
    public TeamsAreAlreadyPairedException() : base() {}

    public TeamsAreAlreadyPairedException(string? message) : base(message) {}

    public TeamsAreAlreadyPairedException(Guid tournamentId) : base($"The teams of the tournament with id: {tournamentId} are already paired.") {}

    public TeamsAreAlreadyPairedException(string? message, Exception? innerException) : base(message, innerException) {}
}