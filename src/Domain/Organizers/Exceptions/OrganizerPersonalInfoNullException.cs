using Domain.Common.Exceptions;

namespace Domain.Organizers.Exceptions;
public sealed class OrganizerPersonalInfoNullException : BadRequestException
{
    public OrganizerPersonalInfoNullException() : base() {}

    public OrganizerPersonalInfoNullException(string? message) : base(message) {}

    public OrganizerPersonalInfoNullException(string? message, Exception? innerException) : base(message, innerException) {}

    public static void ThrowIfNull(OrganizerPersonalInfo? personalInfo, string message = "")
        => _ = personalInfo ?? throw new OrganizerPersonalInfoNullException(message);
}