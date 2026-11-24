using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecnoEdu.Models.Entidades;

namespace TecnoEdu.Data.Configurations;

public class MensajeConfiguration : IEntityTypeConfiguration<Mensaje>
{
    public void Configure(EntityTypeBuilder<Mensaje> builder)
    {
        builder.ToTable("MensajesConsulta");
        builder.HasKey(m => m.IdMensaje);
        builder.HasOne(m => m.Consulta)
            .WithMany(c => c.Mensajes)
            .HasForeignKey(m => m.IdConsulta)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.Remitente)
            .WithMany()
            .HasForeignKey(m => m.UserIdRemitente)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(m => m.UserIdRemitente).HasMaxLength(450).IsRequired();
        builder.Property(m => m.Contenido).IsRequired();
        builder.Property(m => m.FechaEnvio).HasColumnType("datetime2");
        builder.HasIndex(m => new { m.IdConsulta, m.FechaEnvio });
    }
}
