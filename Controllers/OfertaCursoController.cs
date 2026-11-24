using Microsoft.AspNetCore.Mvc;
using TecnoEdu.Services;

namespace TecnoEdu.Controllers;

[ApiController]
[Route("api/ofertas")]
public class OfertaCursoController : ControllerBase
{
    private readonly IDisponibilidadCuposService _cupos;
    public OfertaCursoController(IDisponibilidadCuposService cupos) => _cupos = cupos;

    [HttpGet("{idOferta:int}/cupos")]
    public async Task<IActionResult> Cupos(int idOferta, CancellationToken ct)
    {
        var disponibles = await _cupos.ConsultarDisponiblesAsync(idOferta, ct);
        if (disponibles is null)
            return NotFound(new { mensaje = "Oferta inexistente o cerrada." });
        return Ok(new { idOferta, cuposDisponibles = disponibles.Value });
    }
}
