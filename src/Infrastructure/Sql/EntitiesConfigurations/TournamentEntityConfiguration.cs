using Domain.Organizers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Sql.EntitiesConfigurations;
public class TournamentEntityConfiguration : IEntityTypeConfiguration<Tournament>
{
    public void Configure(EntityTypeBuilder<Tournament> builder)
    {
        builder.HasKey(t => t.Id).HasName("TournamentId");

        builder.Ignore(t => t.DomainEvents);

        builder
            .HasOne(t => t.Organizer)
            .WithOne(o => o.Tournament)
            .HasForeignKey<Tournament>(t => t.OrganizerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(t => t.Teams)
            .WithOne(t => t.Tournament);

        builder
            .HasMany(t => t.Matches)
            .WithOne(m => m.Tournament);

        builder
            .Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("TournamentName");
    }
}
