using Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Sql.EntitiesConfigurations;
public class TeamEntityConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasKey(t => t.Id).HasName("TeamId");
        builder.Ignore(t => t.DomainEvents);
        builder.HasMany(t => t.Players).WithOne(p => p.Team);
        builder.HasOne(t => t.Manager)
                .WithOne(m => m.Team).HasForeignKey<Team>(t => t.ManagerId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(t => t.Tournament).WithMany(t => t.Teams);
        builder.Property(t => t.Name).IsRequired().HasMaxLength(100).HasColumnName("TeamName");
    }
}
