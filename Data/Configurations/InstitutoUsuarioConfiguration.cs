using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecnoEdu.Models.Entidades;

namespace TecnoEdu.Data.Configurations;

public class InstitutoUsuarioConfiguration : IEntityTypeConfiguration<InstitutoUsuario>
{
    public void Configure(EntityTypeBuilder<InstitutoUsuario> builder)
    {
        builder.ToTable("InstitutoUsuarios");
        builder.HasKey(i => i.IdInstitutoUsuario);
        builder.HasOne(i => i.Instituto)
            .WithMany(i => i.InstitutoUsuarios)
            .HasForeignKey(i => i.IdInstituto)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(i => i.Usuario)
            .WithMany()
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(i => i.UserId).HasMaxLength(450).IsRequired();
        builder.Property(i => i.RolInterno).HasMaxLength(40).IsRequired();
        builder.HasIndex(i => new { i.IdInstituto, i.UserId }).IsUnique();
    }
}
