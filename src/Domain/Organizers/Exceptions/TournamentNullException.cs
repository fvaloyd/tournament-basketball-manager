using Domain.Common.Exceptions;

namespace Domain.Organizers.Exceptions;
public sealed class TournamentNullException : BadRequestException
{
    public TournamentNullException() : base()
    {
    }

    public TournamentNullException(string? message) : base(message)
    {
    }

    public TournamentNullException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public static void ThrowIfNull(Tournament? tournament, string message = "")
        => _ = tournament ?? throw new TournamentNullException(message);
}