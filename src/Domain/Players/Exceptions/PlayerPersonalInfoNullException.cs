using Domain.Common.Exceptions;

namespace Domain.Players.Exceptions;

public sealed class PlayerPersonalInfoNullException : BadRequestException
{
    public PlayerPersonalInfoNullException() : base() {}

    public PlayerPersonalInfoNullException(string? message) : base(message) {}

    public PlayerPersonalInfoNullException(string? message, Exception? innerException) : base(message, innerException) {}

    public static void ThrowIfNull(PlayerPersonalInfo? personalInfo, string message = "")
        => _ = personalInfo ?? throw new PlayerPersonalInfoNullException(message);
}