namespace TecnoEdu.Models.ViewModels
{
    public class CourseFilterViewModel
    {
        public string Search { get; set; } = string.Empty;
        public string Modality { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public List<CategoryItem> Categories { get; set; } = new List<CategoryItem>();
        public List<string> Modalities { get; set; } = new List<string>();

        public bool HasFilters => Search != "" || Modality != "" || CategoryId != null;
    }
}
