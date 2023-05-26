using Domain.Common.Exceptions;

namespace Domain.Organizers.Exceptions;
public sealed class OrganizerDoesNotHaveTournamentException : BadRequestException
{
    public OrganizerDoesNotHaveTournamentException() : base("Organizer does not have a tournament yet.") {}

    public OrganizerDoesNotHaveTournamentException(string? message) : base(message) {}

    public OrganizerDoesNotHaveTournamentException(string? message, Exception? innerException) : base(message, innerException) {}
}