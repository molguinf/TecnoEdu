using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecnoEdu.Models.Entities;

namespace TecnoEdu.Data.Configurations;

public class InquiryConfiguration : IEntityTypeConfiguration<Inquiry>
{
    public void Configure(EntityTypeBuilder<Inquiry> builder)
    {
        builder.ToTable("Inquiries");
        builder.HasKey(c => c.InquiryId);
        builder.HasOne(c => c.Institute)
            .WithMany(i => i.Inquiries)
            .HasForeignKey(c => c.InstituteId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(c => c.Course)
            .WithMany(c => c.Inquiries)
            .HasForeignKey(c => c.CourseId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
        builder.Property(c => c.UserId).HasMaxLength(450).IsRequired();
        builder.Property(c => c.Subject).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Status).HasMaxLength(20).IsRequired();
        builder.Property(c => c.CreatedAt).HasColumnType("datetime2");
        builder.HasIndex(c => c.InstituteId);
        builder.HasIndex(c => c.CourseId);
    }
}
