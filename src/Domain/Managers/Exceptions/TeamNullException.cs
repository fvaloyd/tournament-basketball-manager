using Domain.Common.Exceptions;

namespace Domain.Managers.Exceptions;
public sealed class TeamNullException : BadRequestException
{
    public TeamNullException() : base()
    {
    }

    public TeamNullException(string? message) : base(message)
    {
    }

    public TeamNullException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public static void ThrowIfNull(Team? team)
        => _ = team ?? throw new TeamNullException();
}