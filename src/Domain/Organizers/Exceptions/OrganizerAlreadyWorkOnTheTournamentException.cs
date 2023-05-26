using Domain.Common.Exceptions;

namespace Domain.Organizers.Exceptions;
public sealed class OrganizerAlreadyWorkOnTheTournamentException : BadRequestException
{
    public OrganizerAlreadyWorkOnTheTournamentException() : base() { }

    public OrganizerAlreadyWorkOnTheTournamentException(string? message) : base(message) { }

    public OrganizerAlreadyWorkOnTheTournamentException(string? message, Exception? innerException) : base(message, innerException) { }

    public OrganizerAlreadyWorkOnTheTournamentException(Guid organizerId) : base($"The organizer with the id: {organizerId} already work on the tournament.") { }
}