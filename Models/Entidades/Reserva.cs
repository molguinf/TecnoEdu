using System.ComponentModel.DataAnnotations;

namespace TecnoEdu.Models.Entidades;

public class Reserva
{
    [Key]
    public int IdReserva { get; set; }

    [Range(1, int.MaxValue)]
    public int IdOferta { get; set; }

    [Required]
    [StringLength(450)]
    public string UserId { get; set; } = string.Empty;

    [DataType(DataType.DateTime)]
    public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;

    [Required]
    [StringLength(20)]
    public string Estado { get; set; } = "Confirmada";

    [StringLength(500)]
    public string? Observacion { get; set; }

    public OfertaCurso OfertaCurso { get; set; } = null!;
    public ApplicationUser Usuario { get; set; } = null!;
}
