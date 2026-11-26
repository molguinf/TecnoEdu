using System.ComponentModel.DataAnnotations;

namespace TecnoEdu.Models.Entities;

// Modelo de apoyo necesario para materializar las FK del diagrama.
// Confirmar con I1/I2 si ya tienen su propia implementación de Institute.
public class Institute
{
    [Key]
    public int InstituteId { get; set; }

    [Required, StringLength(180)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required, StringLength(250)]
    public string Address { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string City { get; set; } = string.Empty;

    [Required, StringLength(30)]
    public string Phone { get; set; } = string.Empty;

    [Required, StringLength(150), EmailAddress]
    public string ContactEmail { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Website { get; set; }

    [StringLength(500)]
    public string? LogoUrl { get; set; }

    [Required, StringLength(20)]
    public string VerificationStatus { get; set; } = "Pending";

    public ICollection<InstituteUser> InstituteUsers { get; set; } = new List<InstituteUser>();
    public ICollection<Course> Courses { get; set; } = new List<Course>();
    public ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();
}
