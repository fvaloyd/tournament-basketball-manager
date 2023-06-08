using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure;

public static class EntityTypeConfigurationHelper
{
    public static void ConfigurePersonalInfo<TEntity, TEntityPersonalInfo>(OwnedNavigationBuilder<TEntity, TEntityPersonalInfo> personalInfo) 
        where TEntity : Entity
        where TEntityPersonalInfo : PersonalInfo
    {
            personalInfo.Property(x => x.FirstName).HasColumnName("FirstName");
            personalInfo.Property(x => x.LastName).HasColumnName("LastName");
            personalInfo.Property(x => x.DateOfBirth).HasColumnName("DateOfBirth");
            personalInfo.Property(x => x.Email).HasColumnName("Email");
            personalInfo.Property(x => x.Country).HasColumnName("Country");
            personalInfo.Property(x => x.City).HasColumnName("City");
            personalInfo.Property(x => x.Street).HasColumnName("Street");
            personalInfo.Property(x => x.Code).HasColumnName("PostalCode");
            personalInfo.Property(x => x.HouseNumber).HasColumnName("HouseNumber");
    }
}