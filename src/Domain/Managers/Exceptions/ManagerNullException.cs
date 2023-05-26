using Domain.Common.Exceptions;

namespace Domain.Managers.Exceptions;
public sealed class ManagerNullException : BadRequestException
{
    public ManagerNullException() : base()
    {
    }

    public ManagerNullException(string? message) : base(message)
    {
    }

    public ManagerNullException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public static void ThrowIfNull(Manager? manager, string message = "")
        => _ = manager ?? throw new ManagerNullException(message);
}