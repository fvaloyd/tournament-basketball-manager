using Domain.Common.Exceptions;

namespace Domain.Organizers.Exceptions;
public class NumberOfTeamIsNotEvenException : BadRequestException
{
    public NumberOfTeamIsNotEvenException()
    {
    }

    public NumberOfTeamIsNotEvenException(string? message) : base(message)
    {
    }

    public NumberOfTeamIsNotEvenException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
