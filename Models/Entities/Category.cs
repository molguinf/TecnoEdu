using System.ComponentModel.DataAnnotations;

namespace TecnoEdu.Models.Entities;

public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Description { get; set; }

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
