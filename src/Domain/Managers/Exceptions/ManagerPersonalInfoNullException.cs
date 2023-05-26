using Domain.Common.Exceptions;

namespace Domain.Managers.Exceptions;
public sealed class ManagerPersonalInfoNullException : BadRequestException
{
    public ManagerPersonalInfoNullException() : base()
    {
    }

    public ManagerPersonalInfoNullException(string? message) : base(message)
    {
    }

    public ManagerPersonalInfoNullException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public static void ThrowIfNull(ManagerPersonalInfo? personalInfo, string message = "")
        => _ = personalInfo ?? throw new ManagerPersonalInfoNullException(message);
}