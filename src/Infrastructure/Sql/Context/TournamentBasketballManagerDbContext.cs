using Domain.Players;
using Domain.Managers;
using Domain.Organizers;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Sql.EntitiesConfigurations;

namespace Infrastructure.Sql.Context;
public class TournamentBasketballManagerDbContext : DbContext
{
    public TournamentBasketballManagerDbContext(DbContextOptions<TournamentBasketballManagerDbContext> options)
        : base(options) {}

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
}