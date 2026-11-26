namespace TecnoEdu.Models.ViewModels
{
    public class CourseCatalogViewModel
    {
        public CourseFilterViewModel Filters { get; set; } = new CourseFilterViewModel();
        public List<CourseCard> Courses { get; set; } = new List<CourseCard>();
        public int TotalCourses { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}
