using Domain.Managers;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Repositories;
public class SqlTeamRepository : SqlRepositoryBase<Team>, ITeamRepository
{
    public SqlTeamRepository(TournamentBasketballManagerDbContext db) : base(db) {}

    public Task<Team> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => FindByCondition(t => t.Id == id).SingleOrDefaultAsync(cancellationToken);
}