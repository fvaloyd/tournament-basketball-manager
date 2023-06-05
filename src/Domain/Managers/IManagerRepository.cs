namespace Domain.Managers;
public interface IManagerRepository
{
    Task<Manager> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task CreateManagerAsync(Manager manager, CancellationToken cancellationToken = default);
}