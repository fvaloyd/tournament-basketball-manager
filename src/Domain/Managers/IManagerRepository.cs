using System.Linq.Expressions;

namespace Domain.Managers;
public interface IManagerRepository
{
    Task<Manager> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task CreateAsync(Manager manager, CancellationToken cancellationToken = default);
    Task UpdateAsync(Manager managerUpdated, CancellationToken cancellationToken);
    Task<IEnumerable<Manager>> GetByIds(IEnumerable<Guid> ids, CancellationToken cancellationToken);
}