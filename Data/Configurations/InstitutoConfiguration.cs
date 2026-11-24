using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecnoEdu.Models.Entidades;

namespace TecnoEdu.Data.Configurations;

// Configuración de apoyo: coordinar con el responsable de Institutos.
public class InstitutoConfiguration : IEntityTypeConfiguration<Instituto>
{
    public void Configure(EntityTypeBuilder<Instituto> builder)
    {
        builder.ToTable("Institutos");
        builder.HasKey(i => i.IdInstituto);
        builder.Property(i => i.Nombre).HasMaxLength(180).IsRequired();
        builder.Property(i => i.Direccion).HasMaxLength(250).IsRequired();
        builder.Property(i => i.Ciudad).HasMaxLength(100).IsRequired();
        builder.Property(i => i.Telefono).HasMaxLength(30).IsRequired();
        builder.Property(i => i.EmailContacto).HasMaxLength(150).IsRequired();
        builder.Property(i => i.SitioWeb).HasMaxLength(250);
        builder.Property(i => i.LogoUrl).HasMaxLength(500);
        builder.Property(i => i.Descripcion).IsRequired();
        builder.Property(i => i.EstadoVerificacion).HasMaxLength(20).IsRequired();
        builder.HasIndex(i => i.Nombre);
    }
}
