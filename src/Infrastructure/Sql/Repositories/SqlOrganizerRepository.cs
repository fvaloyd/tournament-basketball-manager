using Domain.Organizers;
using Infrastructure.Sql.Context;
using Domain.Organizers.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Repositories;
public class SqlOrganizerRepository : IOrganizerRepository
{
    private readonly TournamentBasketballManagerDbContext _db;
    public SqlOrganizerRepository(TournamentBasketballManagerDbContext db) => _db = db;

    public async Task CreateAsync(Organizer organizer, CancellationToken cancellationToken = default) => await _db.Organizers.AddAsync(organizer, cancellationToken);

    public async Task<Organizer> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var organizer = await _db.Organizers.Include(o => o.Tournament).SingleOrDefaultAsync(o => o.Id == id);
        return organizer is null
            ? throw new OrganizerNotFoundException(id)
            : organizer;
    }

    public async Task<IEnumerable<Match>> GetTournamentMatches(Guid organizerId, CancellationToken cancellationToken = default)
    {
        var tournament = await _db.Tournaments.SingleOrDefaultAsync(t => t.OrganizerId == organizerId) ?? throw new TournamentNotFoundException("The organizer has no team assigned.");
        var matches = await _db.Matches.Where(m => m.TournamentId == tournament.Id).ToListAsync();
        return matches.Any()
            ? matches
            : throw new TeamsAreNotPairedYetException();
    }
}