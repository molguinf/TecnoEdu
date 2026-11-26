using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TecnoEdu.Models
{
    public class Institute
    {
        public int Id { get; set; }

        [BindNever]
        [ValidateNever]
        [Required]
        [StringLength(450)]
        public string OwnerId { get; set; } = "";

        [Required]
        [StringLength(150)]
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
        [StringLength(256)]
        public string Email { get; set; } = "";

        [Required]
        [Phone]
        [StringLength(30)]
        public string Phone { get; set; } = "";
    }
}
