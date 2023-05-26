using Domain.Common.Exceptions;

namespace Domain.Organizers.Exceptions;
public sealed class TournamentNotFoundException : NotFoundException
{
    public TournamentNotFoundException() : base() {}

    public TournamentNotFoundException(string? message) : base(message) {}

    public TournamentNotFoundException(Guid tournamentId) : base($"The tournament with id: {tournamentId} was not found.") {}

    public TournamentNotFoundException(string? message, Exception? innerException) : base(message, innerException) {}
}