using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecnoEdu.Models.Entities;

namespace TecnoEdu.Data.Configurations;

public class CourseOfferingConfiguration : IEntityTypeConfiguration<CourseOffering>
{
    public void Configure(EntityTypeBuilder<CourseOffering> builder)
    {
        builder.ToTable("CourseOfferings", tabla =>
        {
            tabla.HasCheckConstraint("CK_CourseOffering_Capacity", "[TotalCapacity] > 0");
            tabla.HasCheckConstraint("CK_CourseOffering_Price", "[Price] >= 0");
            tabla.HasCheckConstraint("CK_CourseOffering_Dates", "[EndDate] IS NULL OR [EndDate] >= [StartDate]");
        });
        builder.HasKey(o => o.OfferingId);
        builder.HasOne(o => o.Course)
            .WithMany(c => c.Offerings)
            .HasForeignKey(o => o.CourseId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(o => o.StartDate).HasColumnType("date");
        builder.Property(o => o.EndDate).HasColumnType("date");
        builder.Property(o => o.Price).HasPrecision(12, 2);
        builder.Property(o => o.ScheduleDescription).HasMaxLength(200).IsRequired();
        builder.Property(o => o.Currency).HasMaxLength(3).IsRequired();
        builder.Property(o => o.Status).HasMaxLength(20).IsRequired();
        builder.HasIndex(o => o.CourseId);
    }
}
