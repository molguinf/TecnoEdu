using System.ComponentModel.DataAnnotations;

namespace TecnoEdu.Models.Entidades;

public class Categoria
{
    [Key]
    public int IdCategoria { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Descripcion { get; set; }

    public ICollection<Curso> Cursos { get; set; } = new List<Curso>();
}
