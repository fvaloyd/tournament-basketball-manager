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

    public async Task<IEnumerable<Organizer>> GetAllOrganizersAsync(CancellationToken cancellationToken = default)
    {
        var organizer = await _db.Organizers
            .Include(o => o.Tournament).ThenInclude(t => t!.Matches)
            .Include(o => o.Tournament).ThenInclude(t => t!.Teams)
            .ToListAsync(cancellationToken);
        return organizer ?? Enumerable.Empty<Organizer>();
    }

    public async Task<Organizer> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var organizer = await _db.Organizers
                    .Include(o => o.Tournament).ThenInclude(t => t!.Matches)
                    .Include(o => o.Tournament).ThenInclude(t => t!.Teams)
                    .SingleOrDefaultAsync(o => o.Id == id, cancellationToken: cancellationToken);
        return organizer ?? throw new OrganizerNotFoundException(id);
    }

    public Task UpdateAsync(Organizer organizerUpdated, CancellationToken cancellationToken)
    {
        _db.Organizers.Update(organizerUpdated);
        return Task.CompletedTask;
    }
}