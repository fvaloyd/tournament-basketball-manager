using Domain.Common.Exceptions;

namespace Domain.Organizers.Exceptions;
public sealed class TeamAlreadyInTournamentException : BadRequestException
{
    public TeamAlreadyInTournamentException() : base() {}

    public TeamAlreadyInTournamentException(string? message) : base(message) {}

    public TeamAlreadyInTournamentException(Guid teamId) : base($"The team with id: {teamId} is already registered in the tournament") {}

    public TeamAlreadyInTournamentException(string? message, Exception? innerException) : base(message, innerException) {}
}