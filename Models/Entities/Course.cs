using System.ComponentModel.DataAnnotations;

namespace TecnoEdu.Models.Entities;

public class Course
{
    [Key]
    public int CourseId { get; set; }

    [Range(1, int.MaxValue)]
    public int InstituteId { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    [Required]
    [StringLength(180)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public string? Requirements { get; set; }

    [Range(1, int.MaxValue)]
    public int? DurationHours { get; set; }

    [Required]
    [StringLength(30)]
    public string Modality { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public Institute Institute { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public ICollection<CourseOffering> Offerings { get; set; } = new List<CourseOffering>();
    public ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();
}
