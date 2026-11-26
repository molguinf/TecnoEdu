namespace TecnoEdu.Models.ViewModels
{
    public class CourseCard
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string InstituteName { get; set; } = string.Empty;
        public int? DurationHours { get; set; }
        public string Modality { get; set; } = string.Empty;
    }
}
