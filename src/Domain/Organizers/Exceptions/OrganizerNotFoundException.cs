using Domain.Common.Exceptions;

namespace Domain.Organizers.Exceptions;
public sealed class OrganizerNotFoundException : NotFoundException
{
    public OrganizerNotFoundException() : base() {}

    public OrganizerNotFoundException(string? message) : base(message) {}

    public OrganizerNotFoundException(Guid organizerId) : base($"The organizer with id: {organizerId} was not found.") {}

    public OrganizerNotFoundException(string? message, Exception? innerException) : base(message, innerException) {}
}