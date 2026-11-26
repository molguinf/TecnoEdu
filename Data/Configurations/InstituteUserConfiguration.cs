using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecnoEdu.Models.Entities;

namespace TecnoEdu.Data.Configurations;

public class InstituteUserConfiguration : IEntityTypeConfiguration<InstituteUser>
{
    public void Configure(EntityTypeBuilder<InstituteUser> builder)
    {
        builder.ToTable("InstituteUsers");
        builder.HasKey(i => i.InstituteUserId);
        builder.HasOne(i => i.Institute)
            .WithMany(i => i.InstituteUsers)
            .HasForeignKey(i => i.InstituteId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(i => i.User)
            .WithMany()
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(i => i.UserId).HasMaxLength(450).IsRequired();
        builder.Property(i => i.InternalRole).HasMaxLength(40).IsRequired();
        builder.HasIndex(i => new { i.InstituteId, i.UserId }).IsUnique();
    }
}
