using Domain.Players;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Repositories;
public class SqlPlayerRepository : SqlRepositoryBase<Player>, IPlayerRepository
{
    public SqlPlayerRepository(TournamentBasketballManagerDbContext db)
        : base(db) {}

    public Task CreateAsync(Player player, CancellationToken cancellationToken = default)
    {
        Create(player);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<Player>> GetAllAsync(CancellationToken cancellationToken = default)
        => await FindAll().ToListAsync();

    public Task<Player> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => FindByCondition(p => p.Id == id).SingleOrDefaultAsync();
}