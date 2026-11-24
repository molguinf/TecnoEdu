using System.ComponentModel.DataAnnotations;

namespace TecnoEdu.Models.Entidades;

public class Mensaje
{
    [Key]
    public int IdMensaje { get; set; }

    [Range(1, int.MaxValue)]
    public int IdConsulta { get; set; }

    [Required]
    [StringLength(450)]
    public string UserIdRemitente { get; set; } = string.Empty;

    [Required]
    public string Contenido { get; set; } = string.Empty;

    [DataType(DataType.DateTime)]
    public DateTime FechaEnvio { get; set; } = DateTime.UtcNow;

    public Consulta Consulta { get; set; } = null!;
    public ApplicationUser Remitente { get; set; } = null!;
}
