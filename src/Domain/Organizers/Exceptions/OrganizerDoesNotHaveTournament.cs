using Domain.Common.Exceptions;

namespace Domain.Organizers.Exceptions;
public sealed class OrganizerDoesNotHaveTournament : BadRequestException
{
    public OrganizerDoesNotHaveTournament() : base("Organizer does not have a tournament yet.") {}

    public OrganizerDoesNotHaveTournament(string? message) : base(message) {}

    public OrganizerDoesNotHaveTournament(string? message, Exception? innerException) : base(message, innerException) {}
}