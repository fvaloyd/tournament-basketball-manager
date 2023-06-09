namespace Domain.Managers;
public interface IManagerRepository
{
    Task<Manager> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task CreateAsync(Manager manager, CancellationToken cancellationToken = default);
}