using Domain.Common.Exceptions;

namespace Domain.Organizers.Exceptions;
public sealed class OrganizerAlreadyParticipateInTheTournamentException : BadRequestException
{
    public OrganizerAlreadyParticipateInTheTournamentException() : base() {}

    public OrganizerAlreadyParticipateInTheTournamentException(string? message) : base(message) {}

    public OrganizerAlreadyParticipateInTheTournamentException(Guid organizerId, Guid tournamentId) : base($"The organizer with id: {organizerId} is already participating in the tournament with id: {tournamentId}") {}

    public OrganizerAlreadyParticipateInTheTournamentException(string? message, Exception? innerException) : base(message, innerException) {}
}