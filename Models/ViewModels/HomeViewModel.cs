namespace TecnoEdu.Models.ViewModels
{
    public class HomeViewModel
    {
        public CourseFilterViewModel Filters { get; set; } = new CourseFilterViewModel();
        public List<CourseCard> LatestCourses { get; set; } = new List<CourseCard>();
    }
}
