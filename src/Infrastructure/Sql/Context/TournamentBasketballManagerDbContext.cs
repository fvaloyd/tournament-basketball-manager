using MassTransit;
using Domain.Common;
using Domain.Players;
using Domain.Managers;
using Domain.Organizers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Context;
public class TournamentBasketballManagerDbContext : DbContext
{
    public TournamentBasketballManagerDbContext(DbContextOptions<TournamentBasketballManagerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Manager> Managers => Set<Manager>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Organizer> Organizers => Set<Organizer>();
    public DbSet<Tournament> Tournaments => Set<Tournament>();
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<Player> Players => Set<Player>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InfrastructureReference).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entities = ChangeTracker
            .Entries<Entity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any());
        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToArray();
        var result = await base.SaveChangesAsync(cancellationToken);
        foreach(var entity in entities)
        {
            entity.ClearEvents();
        }
        return result;
    }
}