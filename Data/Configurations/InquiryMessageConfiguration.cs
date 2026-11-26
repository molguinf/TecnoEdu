using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecnoEdu.Models.Entities;

namespace TecnoEdu.Data.Configurations;

public class InquiryMessageConfiguration : IEntityTypeConfiguration<InquiryMessage>
{
    public void Configure(EntityTypeBuilder<InquiryMessage> builder)
    {
        builder.ToTable("InquiryMessages");
        builder.HasKey(m => m.MessageId);
        builder.HasOne(m => m.Inquiry)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.InquiryId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderUserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(m => m.SenderUserId).HasMaxLength(450).IsRequired();
        builder.Property(m => m.Content).IsRequired();
        builder.Property(m => m.SentAt).HasColumnType("datetime2");
        builder.HasIndex(m => new { m.InquiryId, m.SentAt });
    }
}
