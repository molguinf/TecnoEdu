using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecnoEdu.Models.Entidades;

namespace TecnoEdu.Data.Configurations;

public class ConsultaConfiguration : IEntityTypeConfiguration<Consulta>
{
    public void Configure(EntityTypeBuilder<Consulta> builder)
    {
        builder.ToTable("Consultas");
        builder.HasKey(c => c.IdConsulta);
        builder.HasOne(c => c.Instituto)
            .WithMany(i => i.Consultas)
            .HasForeignKey(c => c.IdInstituto)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(c => c.Usuario)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(c => c.Curso)
            .WithMany(c => c.Consultas)
            .HasForeignKey(c => c.IdCurso)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
        builder.Property(c => c.UserId).HasMaxLength(450).IsRequired();
        builder.Property(c => c.Asunto).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Estado).HasMaxLength(20).IsRequired();
        builder.Property(c => c.FechaCreacion).HasColumnType("datetime2");
        builder.HasIndex(c => c.IdInstituto);
        builder.HasIndex(c => c.IdCurso);
    }
}
