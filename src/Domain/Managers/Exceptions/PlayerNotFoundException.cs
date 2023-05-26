using Domain.Common.Exceptions;

namespace Domain.Managers.Exceptions;
public sealed class PlayerNotFoundException : NotFoundException
{
    public PlayerNotFoundException() : base() {}

    public PlayerNotFoundException(Guid id) : base($"Player with id: {id} was not found.") {}

    public PlayerNotFoundException(string? message, Exception? innerException) : base(message, innerException) {}

    public PlayerNotFoundException(string? message) : base(message) {}
}