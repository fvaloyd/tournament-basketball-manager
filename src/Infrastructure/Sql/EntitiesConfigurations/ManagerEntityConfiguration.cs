using Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Sql.EntitiesConfigurations;
public class ManagerEntityConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.HasKey(m => m.Id).HasName("ManagerId");
        builder.Ignore(m => m.DomainEvents);
        builder.HasOne(m => m.Team)
            .WithOne(t => t.Manager);
        builder.OwnsOne(m => m.PersonalInfo, personalInfo => {
            EntityTypeConfigurationHelper.ConfigurePersonalInfo(personalInfo);
        });
    }
}
