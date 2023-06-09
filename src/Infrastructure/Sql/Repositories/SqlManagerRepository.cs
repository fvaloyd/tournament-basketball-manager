using Domain.Managers;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Repositories;
public class SqlManagerRepository : SqlRepositoryBase<Manager>, IManagerRepository
{
    public SqlManagerRepository(TournamentBasketballManagerDbContext db) : base(db) {}

    public Task CreateAsync(Manager manager, CancellationToken cancellationToken = default)
    {
        Create(manager);
        return Task.CompletedTask;
    }

    public Task<Manager> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => FindByCondition(m => m.Id == id).SingleOrDefaultAsync(cancellationToken);
}