using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecnoEdu.Models.Entities;

namespace TecnoEdu.Data.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses");
        builder.HasKey(c => c.CourseId);
        builder.HasOne(c => c.Institute)
            .WithMany(i => i.Courses)
            .HasForeignKey(c => c.InstituteId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(c => c.Category)
            .WithMany(c => c.Courses)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(c => c.InstituteId);
        builder.HasIndex(c => c.CategoryId);
        builder.Property(c => c.Title).HasMaxLength(180).IsRequired();
        builder.Property(c => c.Description).IsRequired();
        builder.Property(c => c.Modality).HasMaxLength(30).IsRequired();
    }
}
