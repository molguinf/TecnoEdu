using System.ComponentModel.DataAnnotations;

namespace TecnoEdu.Models.Entities;

public class Reservation
{
    [Key]
    public int ReservationId { get; set; }

    [Range(1, int.MaxValue)]
    public int OfferingId { get; set; }

    [Required]
    [StringLength(450)]
    public string UserId { get; set; } = string.Empty;

    [DataType(DataType.DateTime)]
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Confirmed";

    [StringLength(500)]
    public string? Notes { get; set; }

    public CourseOffering CourseOffering { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
}
