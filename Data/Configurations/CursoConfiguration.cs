using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecnoEdu.Models.Entidades;

namespace TecnoEdu.Data.Configurations;

public class CursoConfiguration : IEntityTypeConfiguration<Curso>
{
    public void Configure(EntityTypeBuilder<Curso> builder)
    {
        builder.ToTable("Cursos");
        builder.HasKey(c => c.IdCurso);
        builder.HasOne(c => c.Instituto)
            .WithMany(i => i.Cursos)
            .HasForeignKey(c => c.IdInstituto)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(c => c.Categoria)
            .WithMany(c => c.Cursos)
            .HasForeignKey(c => c.IdCategoria)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(c => c.IdInstituto);
        builder.HasIndex(c => c.IdCategoria);
        builder.Property(c => c.Titulo).HasMaxLength(180).IsRequired();
        builder.Property(c => c.Descripcion).IsRequired();
        builder.Property(c => c.Modalidad).HasMaxLength(30).IsRequired();
    }
}
