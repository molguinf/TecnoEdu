using System.ComponentModel.DataAnnotations;

namespace TecnoEdu.Models.Entidades;

// Modelo de apoyo necesario para materializar las FK del diagrama.
// Confirmar con I1/I2 si ya tienen su propia implementación de Instituto.
public class Instituto
{
    [Key]
    public int IdInstituto { get; set; }

    [Required, StringLength(180)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string Descripcion { get; set; } = string.Empty;

    [Required, StringLength(250)]
    public string Direccion { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Ciudad { get; set; } = string.Empty;

    [Required, StringLength(30)]
    public string Telefono { get; set; } = string.Empty;

    [Required, StringLength(150), EmailAddress]
    public string EmailContacto { get; set; } = string.Empty;

    [StringLength(250)]
    public string? SitioWeb { get; set; }

    [StringLength(500)]
    public string? LogoUrl { get; set; }

    [Required, StringLength(20)]
    public string EstadoVerificacion { get; set; } = "Pendiente";

    public ICollection<InstitutoUsuario> InstitutoUsuarios { get; set; } = new List<InstitutoUsuario>();
    public ICollection<Curso> Cursos { get; set; } = new List<Curso>();
    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
}
