using Domain.Organizers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Sql.EntitiesConfigurations;
public class OrganizerEntityConfiguration : IEntityTypeConfiguration<Organizer>
{
    public void Configure(EntityTypeBuilder<Organizer> builder)
    {
        builder.HasKey(o => o.Id).HasName("OrganizerId");

        builder.Ignore(o => o.DomainEvents);

        builder.OwnsOne(o => o.PersonalInfo, personalInfo => {
            EntityTypeConfigurationHelper.ConfigurePersonalInfo(personalInfo);
        });

        builder
            .HasOne(o => o.Tournament)
            .WithOne(t => t.Organizer)
            .HasForeignKey<Organizer>(o => o.TournamentId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
