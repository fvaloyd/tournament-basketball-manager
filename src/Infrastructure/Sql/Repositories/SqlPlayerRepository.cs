using Domain.Managers.Exceptions;
using Domain.Players;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Repositories;
public class SqlPlayerRepository : IPlayerRepository
{
    private readonly TournamentBasketballManagerDbContext _db;
    public SqlPlayerRepository(TournamentBasketballManagerDbContext db) => _db = db;

    public async Task CreateAsync(Player player, CancellationToken cancellationToken = default) => await _db.Players.AddAsync(player, cancellationToken);

    public async Task<IEnumerable<Player>> GetAllAsync(CancellationToken cancellationToken = default) => await _db.Players.ToListAsync(cancellationToken);

    public async Task<Player> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var player = await _db.Players.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
        return player is null
            ? throw new PlayerNotFoundException(id)
            : player;
    }
}