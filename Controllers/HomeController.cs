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
            var publicCourses = _dbContext.Cursos
                .Where(c => c.Activo && c.Instituto.EstadoVerificacion == "Aprobado");

            var model = new HomeViewModel();

            model.Filters.Categories = await _dbContext.Categorias
                .OrderBy(c => c.Nombre)
                .Select(c => new CategoryItem { CategoryId = c.IdCategoria, Name = c.Nombre })
                .ToListAsync();

            model.Filters.Modalities = await publicCourses
                .Select(c => c.Modalidad)
                .Distinct()
                .OrderBy(m => m)
                .ToListAsync();

            model.LatestCourses = await publicCourses
                .OrderByDescending(c => c.IdCurso)
                .Take(6)
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
