using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecnoEdu.Models.Entidades;

namespace TecnoEdu.Data.Configurations;

public class OfertaCursoConfiguration : IEntityTypeConfiguration<OfertaCurso>
{
    public void Configure(EntityTypeBuilder<OfertaCurso> builder)
    {
        builder.ToTable("OfertasCurso", tabla =>
        {
            tabla.HasCheckConstraint("CK_Oferta_Capacidad", "[CapacidadTotal] > 0");
            tabla.HasCheckConstraint("CK_Oferta_Costo", "[Costo] >= 0");
            tabla.HasCheckConstraint("CK_Oferta_Fechas", "[FechaFin] IS NULL OR [FechaFin] >= [FechaInicio]");
        });
        builder.HasKey(o => o.IdOferta);
        builder.HasOne(o => o.Curso)
            .WithMany(c => c.Ofertas)
            .HasForeignKey(o => o.IdCurso)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(o => o.FechaInicio).HasColumnType("date");
        builder.Property(o => o.FechaFin).HasColumnType("date");
        builder.Property(o => o.Costo).HasPrecision(12, 2);
        builder.Property(o => o.HorarioDescripcion).HasMaxLength(200).IsRequired();
        builder.Property(o => o.Moneda).HasMaxLength(3).IsRequired();
        builder.Property(o => o.Estado).HasMaxLength(20).IsRequired();
        builder.HasIndex(o => o.IdCurso);
    }
}
