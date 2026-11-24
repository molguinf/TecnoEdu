using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TecnoEdu.Models.Entidades;

public class OfertaCurso
{
    [Key]
    public int IdOferta { get; set; }

    [Range(1, int.MaxValue)]
    public int IdCurso { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime FechaInicio { get; set; }

    [DataType(DataType.Date)]
    public DateTime? FechaFin { get; set; }

    [Required]
    [StringLength(200)]
    public string HorarioDescripcion { get; set; } = string.Empty;

    [Precision(12, 2)]
    [Range(typeof(decimal), "0", "9999999999.99")]
    public decimal Costo { get; set; }

    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string Moneda { get; set; } = "BOB";

    [Range(1, int.MaxValue)]
    public int CapacidadTotal { get; set; }

    // Regla de negocio inicial: Abierta permite reservas. Confirmar con el equipo.
    [Required]
    [StringLength(20)]
    public string Estado { get; set; } = "Abierta";

    public Curso Curso { get; set; } = null!;
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
