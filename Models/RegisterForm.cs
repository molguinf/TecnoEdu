using System.ComponentModel.DataAnnotations;

namespace TecnoEdu.Models
{
    public class RegisterForm
    {
        [Required]
        [StringLength(150)]
        [Display(Name = "Nombre completo")]
        public string FullName { get; set; } = "";

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(100, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; } = "";

        [Required]
        [RegularExpression("^(Institute|Student)$")]
        [Display(Name = "Account type")]
        public string AccountType { get; set; } = "Institute";
    }
}
