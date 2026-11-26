using System.ComponentModel.DataAnnotations;

namespace TecnoEdu.Models.ViewModels;

public class InstituteProfileForm
{
    public int Id { get; set; }

    [Required]
    [StringLength(180)]
    public string Name { get; set; } = "";

    [Required]
    [StringLength(2000)]
    public string Description { get; set; } = "";

    [Required]
    [StringLength(100)]
    public string City { get; set; } = "";

    [Required]
    [StringLength(250)]
    public string Address { get; set; } = "";

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = "";

    [Required]
    [Phone]
    [StringLength(30)]
    public string Phone { get; set; } = "";
}
