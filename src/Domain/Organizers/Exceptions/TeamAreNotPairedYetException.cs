using Domain.Common.Exceptions;

namespace Domain.Organizers.Exceptions;
public sealed class TeamsAreNotPairedYetException : NotFoundException
{
    public TeamsAreNotPairedYetException() : base()
    {
    }

    public TeamsAreNotPairedYetException(string? message) : base(message)
    {
    }

    public TeamsAreNotPairedYetException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}