using System.ComponentModel.DataAnnotations;

namespace TecnoEdu.Models.Entidades;

public class Curso
{
    [Key]
    public int IdCurso { get; set; }

    [Range(1, int.MaxValue)]
    public int IdInstituto { get; set; }

    [Range(1, int.MaxValue)]
    public int IdCategoria { get; set; }

    [Required]
    [StringLength(180)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    public string Descripcion { get; set; } = string.Empty;

    public string? Requisitos { get; set; }

    [Range(1, int.MaxValue)]
    public int? DuracionHoras { get; set; }

    [Required]
    [StringLength(30)]
    public string Modalidad { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;

    public Instituto Instituto { get; set; } = null!;
    public Categoria Categoria { get; set; } = null!;
    public ICollection<OfertaCurso> Ofertas { get; set; } = new List<OfertaCurso>();
    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
}
