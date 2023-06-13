using Domain.Players;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Sql.EntitiesConfigurations;
public class PlayerEntityConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(p => p.Id).HasName("PlayerId");

        builder.Ignore(p => p.DomainEvents);

        builder.OwnsOne(p => p.PersonalInfo, personalInfo => {
            EntityTypeConfigurationHelper.ConfigurePersonalInfo(personalInfo);
            personalInfo.Property(x => x.Weight).HasColumnName("Weight");
            personalInfo.Property(x => x.Height).HasColumnName("Height");
        });

        builder
            .HasOne(p => p.Team)
            .WithMany(t => t.Players)
            .OnDelete(DeleteBehavior.SetNull);
    }
}