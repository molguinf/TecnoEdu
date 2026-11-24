using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TecnoEdu.Data;

namespace TecnoEdu.Controllers;

// Respuesta JSON provisional hasta integrar las vistas Razor del equipo.
public class CategoriaController : Controller
{
    private readonly TecnoEduDbContext _db;

    public CategoriaController(TecnoEduDbContext db) => _db = db;

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var categorias = await _db.Categorias.AsNoTracking()
            .OrderBy(c => c.Nombre)
            .Select(c => new { c.IdCategoria, c.Nombre, c.Descripcion })
            .ToListAsync(ct);
        return Json(categorias);
    }
}
