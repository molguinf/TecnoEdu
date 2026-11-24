using System.ComponentModel.DataAnnotations;

namespace TecnoEdu.Models.Entidades;

public class InstitutoUsuario
{
    [Key]
    public int IdInstitutoUsuario { get; set; }

    [Range(1, int.MaxValue)]
    public int IdInstituto { get; set; }

    [Required]
    [StringLength(450)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [StringLength(40)]
    public string RolInterno { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;

    public Instituto Instituto { get; set; } = null!;
    public ApplicationUser Usuario { get; set; } = null!;
}
