using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TecnoEdu.Data;
using TecnoEdu.Models.ViewModels;

namespace TecnoEdu.Controllers
{
    public class InstitutesController : Controller
    {
        private readonly TecnoEduDbContext _dbContext;

        public InstitutesController(TecnoEduDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Institutes?search=tecno&city=Cochabamba
        public async Task<IActionResult> Index(string? search, string? city)
        {
            search = search?.Trim() ?? "";
            city = city?.Trim() ?? "";

            // Institutes that are not approved yet are never shown.
            var approved = _dbContext.Institutes.Where(i => i.VerificationStatus == "Approved");

            var model = new InstituteDirectoryViewModel
            {
                Search = search,
                City = city
            };

            model.Cities = await approved
                .Select(i => i.City.Trim())
                .Where(name => name != "")
                .Distinct()
                .OrderBy(name => name)
                .ToListAsync();

            var institutes = approved;
            if (search != "")
            {
                institutes = institutes.Where(i => i.Name.Contains(search));
            }
            if (city != "")
            {
                institutes = institutes.Where(i => i.City.Trim() == city);
            }

            model.Institutes = await institutes
                .OrderBy(i => i.Name)
                .ThenBy(i => i.InstituteId)
                .Select(i => new InstituteCard
                {
                    InstituteId = i.InstituteId,
                    Name = i.Name,
                    Address = i.Address,
                    City = i.City,
                    Phone = i.Phone,
                    Email = i.ContactEmail
                })
                .ToListAsync();

            return View(model);
        }

        // GET: Institutes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var model = await _dbContext.Institutes
                .Where(i => i.InstituteId == id && i.VerificationStatus == "Approved")
                .Select(i => new InstituteDetailsViewModel
                {
                    InstituteId = i.InstituteId,
                    Name = i.Name,
                    Description = i.Description,
                    Address = i.Address,
                    City = i.City,
                    Phone = i.Phone,
                    Email = i.ContactEmail,
                    Website = i.Website,
                    LogoUrl = i.LogoUrl
                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }

            // Only http/https links are rendered, so a value like "javascript:..." can't run.
            model.Website = SafeUrl(model.Website);
            model.LogoUrl = SafeUrl(model.LogoUrl);

            model.Courses = await _dbContext.Courses
                .Where(c => c.InstituteId == id && c.IsActive)
                .OrderBy(c => c.Title)
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

        private static string? SafeUrl(string? url)
        {
            if (Uri.TryCreate(url?.Trim(), UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                return uri.AbsoluteUri;
            }
            return null;
        }
    }
}
