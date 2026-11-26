using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TecnoEdu.Models.Entities;

// User Identity: AspNetUsers, llave de tipo string/nvarchar(450).
// Este modelo debe consolidarse con el que implemente el responsable de autenticación.
public class ApplicationUser : IdentityUser
{
    [Required, StringLength(150)]
    public string FullName { get; set; } = string.Empty;
}
