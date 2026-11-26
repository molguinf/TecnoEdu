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
            var categories = await _dbContext.Categorias
                .OrderBy(c => c.Nombre)
                .Select(c => new CategoryItem
                {
                    CategoryId = c.IdCategoria,
                    Name = c.Nombre,
                    Description = c.Descripcion,
                    CourseCount = c.Cursos.Count(course => course.Activo
                        && course.Instituto.EstadoVerificacion == "Aprobado")
                })
                .ToListAsync();

            return View(categories);
        }
    }
}
