using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecnoEdu.Models.Entities;

namespace TecnoEdu.Data.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");
        builder.HasKey(r => r.ReservationId);
        builder.HasOne(r => r.CourseOffering)
            .WithMany(o => o.Reservations)
            .HasForeignKey(r => r.OfferingId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(r => r.UserId).HasMaxLength(450).IsRequired();
        builder.Property(r => r.Status).HasMaxLength(20).IsRequired();
        builder.Property(r => r.Notes).HasMaxLength(500);
        builder.Property(r => r.RequestedAt).HasColumnType("datetime2");
        // La regla de negocio permite una sola reserva confirmada por usuario/oferta.
        builder.HasIndex(r => new { r.OfferingId, r.UserId })
            .IsUnique()
            .HasFilter("[Status] = N'Confirmed'");
    }
}
