using System.ComponentModel.DataAnnotations;

namespace TecnoEdu.Models.Entities;

public class Inquiry
{
    [Key]
    public int InquiryId { get; set; }

    [Range(1, int.MaxValue)]
    public int InstituteId { get; set; }

    [Required]
    [StringLength(450)]
    public string UserId { get; set; } = string.Empty;

    public int? CourseId { get; set; }

    [Required]
    [StringLength(200)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Open";

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Institute Institute { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    public Course? Course { get; set; }
    public ICollection<InquiryMessage> Messages { get; set; } = new List<InquiryMessage>();
}
