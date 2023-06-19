using Domain.Organizers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Sql.EntitiesConfigurations;
public class MatchEntityConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.HasKey(m => new {m.TournamentId, m.TeamAId, m.TeamBId});

        builder.Ignore(m => m.DomainEvents);

        builder
            .HasOne(m => m.Tournament)
            .WithMany(t => t.Matches)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(m => m.TeamA)
            .WithOne()
            .OnDelete(DeleteBehavior.NoAction);
        
        builder
            .HasOne(m => m.TeamB)
            .WithOne()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
