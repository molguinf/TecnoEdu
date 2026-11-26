using System.ComponentModel.DataAnnotations;

namespace TecnoEdu.Models.Entities;

public class InquiryMessage
{
    [Key]
    public int MessageId { get; set; }

    [Range(1, int.MaxValue)]
    public int InquiryId { get; set; }

    [Required]
    [StringLength(450)]
    public string SenderUserId { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    [DataType(DataType.DateTime)]
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    public Inquiry Inquiry { get; set; } = null!;
    public ApplicationUser Sender { get; set; } = null!;
}
