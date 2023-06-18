using Domain.Managers;
using Domain.Organizers.Exceptions;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Repositories;
public class SqlTeamRepository : ITeamRepository
{
    private readonly TournamentBasketballManagerDbContext _db;
    public SqlTeamRepository(TournamentBasketballManagerDbContext db) => _db = db;

    public async Task CreateAsync(Team team, CancellationToken cancellationToken = default)
    {
        await _db.Teams.AddAsync(team, cancellationToken);
    }

    public async Task<Team> GetByIdAsync(Guid? id, CancellationToken cancellationToken)
    {
        var team = await _db.Teams.SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
        return team ?? throw new TeamNotFoundException(id);
    }
}