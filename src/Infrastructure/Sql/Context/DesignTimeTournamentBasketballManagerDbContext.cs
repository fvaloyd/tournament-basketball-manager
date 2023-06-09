using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure;

public class DesignTimeTournamentBasketballManagerDbContext : IDesignTimeDbContextFactory<TournamentBasketballManagerDbContext>
{
    public TournamentBasketballManagerDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TournamentBasketballManagerDbContext>();
        optionsBuilder.UseSqlServer("server=FRANCIS\\SQLEXPRESS;user=sa;password=123;database=demoDb;TrustServerCertificate=True;Encrypt=False;");

        return new TournamentBasketballManagerDbContext(optionsBuilder.Options);
    }
}