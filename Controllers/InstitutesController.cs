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
            var approved = _dbContext.Institutos.Where(i => i.EstadoVerificacion == "Aprobado");

            var model = new InstituteDirectoryViewModel
            {
                Search = search,
                City = city
            };

            model.Cities = await approved
                .Select(i => i.Ciudad.Trim())
                .Where(name => name != "")
                .Distinct()
                .OrderBy(name => name)
                .ToListAsync();

            var institutes = approved;
            if (search != "")
            {
                institutes = institutes.Where(i => i.Nombre.Contains(search));
            }
            if (city != "")
            {
                institutes = institutes.Where(i => i.Ciudad.Trim() == city);
            }

            model.Institutes = await institutes
                .OrderBy(i => i.Nombre)
                .ThenBy(i => i.IdInstituto)
                .Select(i => new InstituteCard
                {
                    InstituteId = i.IdInstituto,
                    Name = i.Nombre,
                    Address = i.Direccion,
                    City = i.Ciudad,
                    Phone = i.Telefono,
                    Email = i.EmailContacto
                })
                .ToListAsync();

            return View(model);
        }

        // GET: Institutes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var model = await _dbContext.Institutos
                .Where(i => i.IdInstituto == id && i.EstadoVerificacion == "Aprobado")
                .Select(i => new InstituteDetailsViewModel
                {
                    InstituteId = i.IdInstituto,
                    Name = i.Nombre,
                    Description = i.Descripcion,
                    Address = i.Direccion,
                    City = i.Ciudad,
                    Phone = i.Telefono,
                    Email = i.EmailContacto,
                    Website = i.SitioWeb,
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

            model.Courses = await _dbContext.Cursos
                .Where(c => c.IdInstituto == id && c.Activo)
                .OrderBy(c => c.Titulo)
                .Select(c => new CourseCard
                {
                    CourseId = c.IdCurso,
                    Title = c.Titulo,
                    CategoryId = c.IdCategoria,
                    CategoryName = c.Categoria.Nombre,
                    InstituteName = c.Instituto.Nombre,
                    DurationHours = c.DuracionHoras,
                    Modality = c.Modalidad
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
