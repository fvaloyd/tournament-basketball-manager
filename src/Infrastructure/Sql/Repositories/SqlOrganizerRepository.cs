using Domain.Organizers;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Repositories;
public class SqlOrganizerRepository : SqlRepositoryBase<Organizer>, IOrganizerRepository
{
    public SqlOrganizerRepository(TournamentBasketballManagerDbContext db)
        : base(db) {}

    public Task CreateAsync(Organizer organizer, CancellationToken cancellationToken = default)
    {
        Create(organizer);
        return Task.CompletedTask;
    }

    public Task<Organizer> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => FindByCondition(o => o.Id == id).SingleOrDefaultAsync();

    public async Task<IEnumerable<Match>> GetTournamentMatches(Guid organizerId, CancellationToken cancellationToken = default)
    {
        var organizer = FindByCondition(o => o.Id.Equals(organizerId) && o.IsOrganizingATournament);
        var tournament = organizer.Include(o => o.Tournament).Select(o => o.Tournament);
        var matches = tournament.Include(t => t.Matches).Select(t => t.Matches);
        matches.Include("TeamA, TeamB, Tournament");
        return (IEnumerable<Match>)await matches.ToListAsync();
    }
}