using Domain.Common.Exceptions;

namespace Domain.Managers.Exceptions;
public sealed class TeamAlreadyRegisteredInATournamentException : BadRequestException
{
    public TeamAlreadyRegisteredInATournamentException() : base() {}

    public TeamAlreadyRegisteredInATournamentException(string? message) : base(message) {}

    public TeamAlreadyRegisteredInATournamentException(Guid teamId, Guid tournamentId) : base($"Team with id: {teamId} is already registered in a tournament with the id: {tournamentId}") {}

    public TeamAlreadyRegisteredInATournamentException(string? message, Exception? innerException) : base(message, innerException) {}
}