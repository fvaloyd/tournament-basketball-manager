using Domain.Common.Exceptions;

namespace Domain.Players.Exceptions;
public sealed class PlayerNullException : BadRequestException
{
    public PlayerNullException() : base() {}

    public PlayerNullException(string? message) : base(message) {}

    public PlayerNullException(string? message, Exception? innerException) : base(message, innerException) {}

    public static void ThrowIfNull(Player? player, string message = "")
        => _ = player ?? throw new PlayerNullException(message);
}