using Domain.Common.Exceptions;

namespace Domain.Organizers.Exceptions;
public sealed class OrganizerNullException : BadRequestException
{
    public OrganizerNullException() : base() {}

    public OrganizerNullException(string? message) : base(message) {}

    public OrganizerNullException(string? message, Exception? innerException) : base(message, innerException) {}

    public static void ThrowIfNull(Organizer? organizer, string message = "")
        => _ = organizer ?? throw new OrganizerNullException(message);
}