using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TecnoEdu.Data;
using TecnoEdu.Models.ViewModels;

namespace TecnoEdu.Controllers
{
    public class CoursesController : Controller
    {
        private const int PageSize = 9;
        private readonly TecnoEduDbContext _dbContext;

        public CoursesController(TecnoEduDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Courses?search=web&modality=Presencial&categoryId=1&page=2
        public async Task<IActionResult> Index(string? search, string? modality, int? categoryId, int page = 1)
        {
            search = search?.Trim() ?? "";
            modality = modality?.Trim() ?? "";

            // Only active courses from approved institutes are public.
            var publicCourses = _dbContext.Courses
                .Where(c => c.IsActive && c.Institute.VerificationStatus == "Approved");

            var courses = publicCourses;
            if (search != "")
            {
                courses = courses.Where(c => c.Title.Contains(search)
                    || c.Description.Contains(search)
                    || c.Institute.Name.Contains(search));
            }
            if (modality != "")
            {
                courses = courses.Where(c => c.Modality == modality);
            }
            if (categoryId != null)
            {
                courses = courses.Where(c => c.CategoryId == categoryId);
            }

            int totalCourses = await courses.CountAsync();
            int totalPages = Math.Max(1, (int)Math.Ceiling(totalCourses / (double)PageSize));
            page = Math.Clamp(page, 1, totalPages);

            var model = new CourseCatalogViewModel
            {
                TotalCourses = totalCourses,
                Page = page,
                TotalPages = totalPages
            };

            model.Filters.Search = search;
            model.Filters.Modality = modality;
            model.Filters.CategoryId = categoryId;
            model.Filters.Categories = await _dbContext.Categories
                .OrderBy(c => c.Name)
                .Select(c => new CategoryItem { CategoryId = c.CategoryId, Name = c.Name })
                .ToListAsync();
            model.Filters.Modalities = await publicCourses
                .Select(c => c.Modality)
                .Distinct()
                .OrderBy(m => m)
                .ToListAsync();

            model.Courses = await courses
                .OrderBy(c => c.Title)
                .ThenBy(c => c.CourseId)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
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

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var model = await _dbContext.Courses
                .Where(c => c.CourseId == id && c.IsActive && c.Institute.VerificationStatus == "Approved")
                .Select(c => new CourseDetailsViewModel
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    Requirements = c.Requirements,
                    DurationHours = c.DurationHours,
                    Modality = c.Modality,
                    CategoryId = c.CategoryId,
                    CategoryName = c.Category.Name,
                    InstituteId = c.InstituteId,
                    InstituteName = c.Institute.Name,
                    InstituteCity = c.Institute.City,
                    InstitutePhone = c.Institute.Phone,
                    InstituteEmail = c.Institute.ContactEmail,
                    Offers = c.Offerings
                        .Where(o => o.Status == "Open")
                        .OrderBy(o => o.StartDate)
                        .Select(o => new OfferSummary
                        {
                            OfferId = o.OfferingId,
                            StartDate = o.StartDate,
                            EndDate = o.EndDate,
                            Schedule = o.ScheduleDescription,
                            Price = o.Price,
                            Currency = o.Currency,
                            AvailableSeats = o.TotalCapacity - o.Reservations.Count(r => r.Status == "Confirmed")
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
    }
}
