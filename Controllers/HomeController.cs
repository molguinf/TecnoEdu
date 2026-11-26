using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TecnoEdu.Data;
using TecnoEdu.Models;
using TecnoEdu.Models.ViewModels;

namespace TecnoEdu.Controllers
{
    public class HomeController : Controller
    {
        private readonly TecnoEduDbContext _dbContext;

        public HomeController(TecnoEduDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            // Only active courses from approved institutes are public.
            var publicCourses = _dbContext.Courses
                .Where(c => c.IsActive && c.Institute.VerificationStatus == "Approved");

            var model = new HomeViewModel();

            model.Filters.Categories = await _dbContext.Categories
                .OrderBy(c => c.Name)
                .Select(c => new CategoryItem { CategoryId = c.CategoryId, Name = c.Name })
                .ToListAsync();

            model.Filters.Modalities = await publicCourses
                .Select(c => c.Modality)
                .Distinct()
                .OrderBy(m => m)
                .ToListAsync();

            model.LatestCourses = await publicCourses
                .OrderByDescending(c => c.CourseId)
                .Take(6)
                .Select(c => new CourseCard
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    CategoryId = c.CategoryId,
                    CategoryName = c.Category.Name,
                    InstituteName = c.Institute.Name,
                    DurationHours = c.DurationHours,
                    Modality = c.Modality
                })
                .ToListAsync();

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
