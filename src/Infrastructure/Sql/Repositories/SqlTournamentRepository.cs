using Domain.Organizers;
using Infrastructure.Sql.Context;

namespace Infrastructure.Sql.Repositories;
public class SqlTournamentRepository : ITournamentRepository
{
    private readonly TournamentBasketballManagerDbContext _db;

    public SqlTournamentRepository(TournamentBasketballManagerDbContext db) => _db = db;

    public async Task CreateAsync(Tournament tournament, CancellationToken cancellationToken = default)
    {
        await _db.Tournaments.AddAsync(tournament, cancellationToken);
    }
}
