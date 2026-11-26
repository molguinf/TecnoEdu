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
            var publicCourses = _dbContext.Cursos
                .Where(c => c.Activo && c.Instituto.EstadoVerificacion == "Aprobado");

            var courses = publicCourses;
            if (search != "")
            {
                courses = courses.Where(c => c.Titulo.Contains(search)
                    || c.Descripcion.Contains(search)
                    || c.Instituto.Nombre.Contains(search));
            }
            if (modality != "")
            {
                courses = courses.Where(c => c.Modalidad == modality);
            }
            if (categoryId != null)
            {
                courses = courses.Where(c => c.IdCategoria == categoryId);
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
            model.Filters.Categories = await _dbContext.Categorias
                .OrderBy(c => c.Nombre)
                .Select(c => new CategoryItem { CategoryId = c.IdCategoria, Name = c.Nombre })
                .ToListAsync();
            model.Filters.Modalities = await publicCourses
                .Select(c => c.Modalidad)
                .Distinct()
                .OrderBy(m => m)
                .ToListAsync();

            model.Courses = await courses
                .OrderBy(c => c.Titulo)
                .ThenBy(c => c.IdCurso)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
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

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var model = await _dbContext.Cursos
                .Where(c => c.IdCurso == id && c.Activo && c.Instituto.EstadoVerificacion == "Aprobado")
                .Select(c => new CourseDetailsViewModel
                {
                    CourseId = c.IdCurso,
                    Title = c.Titulo,
                    Description = c.Descripcion,
                    Requirements = c.Requisitos,
                    DurationHours = c.DuracionHoras,
                    Modality = c.Modalidad,
                    CategoryId = c.IdCategoria,
                    CategoryName = c.Categoria.Nombre,
                    InstituteId = c.IdInstituto,
                    InstituteName = c.Instituto.Nombre,
                    InstituteCity = c.Instituto.Ciudad,
                    InstitutePhone = c.Instituto.Telefono,
                    InstituteEmail = c.Instituto.EmailContacto,
                    Offers = c.Ofertas
                        .Where(o => o.Estado == "Abierta")
                        .OrderBy(o => o.FechaInicio)
                        .Select(o => new OfferSummary
                        {
                            OfferId = o.IdOferta,
                            StartDate = o.FechaInicio,
                            EndDate = o.FechaFin,
                            Schedule = o.HorarioDescripcion,
                            Price = o.Costo,
                            Currency = o.Moneda,
                            AvailableSeats = o.CapacidadTotal - o.Reservas.Count(r => r.Estado == "Confirmada")
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
