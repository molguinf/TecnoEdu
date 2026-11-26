using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecnoEdu.Models.Entities;

namespace TecnoEdu.Data.Configurations;

// Configuración de apoyo: coordinar con el responsable de Institutes.
public class InstituteConfiguration : IEntityTypeConfiguration<Institute>
{
    public void Configure(EntityTypeBuilder<Institute> builder)
    {
        builder.ToTable("Institutes");
        builder.HasKey(i => i.InstituteId);
        builder.Property(i => i.Name).HasMaxLength(180).IsRequired();
        builder.Property(i => i.Address).HasMaxLength(250).IsRequired();
        builder.Property(i => i.City).HasMaxLength(100).IsRequired();
        builder.Property(i => i.Phone).HasMaxLength(30).IsRequired();
        builder.Property(i => i.ContactEmail).HasMaxLength(150).IsRequired();
        builder.Property(i => i.Website).HasMaxLength(250);
        builder.Property(i => i.LogoUrl).HasMaxLength(500);
        builder.Property(i => i.Description).IsRequired();
        builder.Property(i => i.VerificationStatus).HasMaxLength(20).IsRequired();
        builder.HasIndex(i => i.Name);
    }
}
