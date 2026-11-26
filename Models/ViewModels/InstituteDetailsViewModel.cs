namespace TecnoEdu.Models.ViewModels
{
    public class InstituteDetailsViewModel
    {
        public int InstituteId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Website { get; set; }
        public string? LogoUrl { get; set; }
        public List<CourseCard> Courses { get; set; } = new List<CourseCard>();
    }
}
