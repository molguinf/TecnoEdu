namespace TecnoEdu.Models.ViewModels
{
    public class CourseDetailsViewModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Requirements { get; set; }
        public int? DurationHours { get; set; }
        public string Modality { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int InstituteId { get; set; }
        public string InstituteName { get; set; } = string.Empty;
        public string InstituteCity { get; set; } = string.Empty;
        public string InstitutePhone { get; set; } = string.Empty;
        public string InstituteEmail { get; set; } = string.Empty;
        public List<OfferSummary> Offers { get; set; } = new List<OfferSummary>();
    }
}
