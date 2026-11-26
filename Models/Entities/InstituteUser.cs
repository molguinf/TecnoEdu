using System.ComponentModel.DataAnnotations;

namespace TecnoEdu.Models.Entities;

public class InstituteUser
{
    [Key]
    public int InstituteUserId { get; set; }

    [Range(1, int.MaxValue)]
    public int InstituteId { get; set; }

    [Required]
    [StringLength(450)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [StringLength(40)]
    public string InternalRole { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public Institute Institute { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
}
