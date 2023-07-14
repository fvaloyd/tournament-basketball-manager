using Domain.Managers;
using Domain.Managers.Exceptions;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Repositories;
public class SqlManagerRepository : IManagerRepository
{
    private readonly TournamentBasketballManagerDbContext _db;
    public SqlManagerRepository(TournamentBasketballManagerDbContext db) => _db = db;

    public async Task CreateAsync(Manager manager, CancellationToken cancellationToken = default) => await _db.Managers.AddAsync(manager, cancellationToken);

    public async Task<IEnumerable<Manager>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var managers = await _db.Managers
            .Include(m => m.Team).ThenInclude(t => t!.Tournament)
            .Include(m => m.Team).ThenInclude(t => t!.Players)
            .ToListAsync(cancellationToken);
        return managers;
    }

    public async Task<Manager> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var manager = await _db.Managers
            .Include(m => m.Team).ThenInclude(t => t!.Tournament)
            .Include(m => m.Team).ThenInclude(t => t!.Players)
            .SingleOrDefaultAsync(m => m.Id == id, cancellationToken);
        return manager ?? throw new ManagerNotFoundException(id);
    }

    public async Task<IEnumerable<Manager>> GetByIds(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        => await _db.Managers
            .Include(m => m.Team).ThenInclude(t => t!.Tournament)
            .Include(m => m.Team).ThenInclude(t => t!.Players).ToListAsync(cancellationToken);

    public Task UpdateAsync(Manager managerUpdated, CancellationToken cancellationToken)
    {
        _db.Managers.Update(managerUpdated);
        return Task.CompletedTask;
    }
}