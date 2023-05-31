using Domain.Common.Exceptions;

namespace Domain.Managers.Exceptions;
public sealed class ManagerNotFoundException : NotFoundException
{
    public ManagerNotFoundException() : base() {}

    public ManagerNotFoundException(string? message) : base(message) {}

    public ManagerNotFoundException(Guid managerId) : base($"Manager with id: {managerId} was not found.") {}

    public ManagerNotFoundException(string? message, Exception? innerException) : base(message, innerException) {}
}