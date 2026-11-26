using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TecnoEdu.Data;
using TecnoEdu.Models.ViewModels;

namespace TecnoEdu.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly TecnoEduDbContext _dbContext;

        public CategoriesController(TecnoEduDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var categories = await _dbContext.Categories
                .OrderBy(c => c.Name)
                .Select(c => new CategoryItem
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    Description = c.Description,
                    CourseCount = c.Courses.Count(course => course.IsActive
                        && course.Institute.VerificationStatus == "Approved")
                })
                .ToListAsync();

            return View(categories);
        }
    }
}
