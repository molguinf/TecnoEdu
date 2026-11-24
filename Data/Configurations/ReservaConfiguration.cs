using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecnoEdu.Models.Entidades;

namespace TecnoEdu.Data.Configurations;

public class ReservaConfiguration : IEntityTypeConfiguration<Reserva>
{
    public void Configure(EntityTypeBuilder<Reserva> builder)
    {
        builder.ToTable("Reservas");
        builder.HasKey(r => r.IdReserva);
        builder.HasOne(r => r.OfertaCurso)
            .WithMany(o => o.Reservas)
            .HasForeignKey(r => r.IdOferta)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(r => r.Usuario)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(r => r.UserId).HasMaxLength(450).IsRequired();
        builder.Property(r => r.Estado).HasMaxLength(20).IsRequired();
        builder.Property(r => r.Observacion).HasMaxLength(500);
        builder.Property(r => r.FechaSolicitud).HasColumnType("datetime2");
        // La regla de negocio permite una sola reserva confirmada por usuario/oferta.
        builder.HasIndex(r => new { r.IdOferta, r.UserId })
            .IsUnique()
            .HasFilter("[Estado] = N'Confirmada'");
    }
}
