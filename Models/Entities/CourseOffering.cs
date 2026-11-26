using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TecnoEdu.Models.Entities;

public class CourseOffering
{
    [Key]
    public int OfferingId { get; set; }

    [Range(1, int.MaxValue)]
    public int CourseId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    [Required]
    [StringLength(200)]
    public string ScheduleDescription { get; set; } = string.Empty;

    [Precision(12, 2)]
    [Range(typeof(decimal), "0", "9999999999.99")]
    public decimal Price { get; set; }

    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string Currency { get; set; } = "BOB";

    [Range(1, int.MaxValue)]
    public int TotalCapacity { get; set; }

    // Regla de negocio inicial: Open permite reservas. Confirmar con el equipo.
    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Open";

    public Course Course { get; set; } = null!;
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
