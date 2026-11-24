using System.ComponentModel.DataAnnotations;

namespace TecnoEdu.Models.Entidades;

public class Consulta
{
    [Key]
    public int IdConsulta { get; set; }

    [Range(1, int.MaxValue)]
    public int IdInstituto { get; set; }

    [Required]
    [StringLength(450)]
    public string UserId { get; set; } = string.Empty;

    public int? IdCurso { get; set; }

    [Required]
    [StringLength(200)]
    public string Asunto { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Estado { get; set; } = "Abierta";

    [DataType(DataType.DateTime)]
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public Instituto Instituto { get; set; } = null!;
    public ApplicationUser Usuario { get; set; } = null!;
    public Curso? Curso { get; set; }
    public ICollection<Mensaje> Mensajes { get; set; } = new List<Mensaje>();
}
