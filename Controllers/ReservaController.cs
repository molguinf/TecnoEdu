using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TecnoEdu.Services;

namespace TecnoEdu.Controllers;

public sealed class CrearReservaRequest
{
    [Range(1, int.MaxValue)]
    public int IdOferta { get; set; }
}

[ApiController]
[Authorize]
[Route("api/reservas")]
public class ReservaController : ControllerBase
{
    private readonly IDisponibilidadCuposService _cupos;
    public ReservaController(IDisponibilidadCuposService cupos) => _cupos = cupos;

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearReservaRequest request,
        CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
        var resultado = await _cupos.ReservarAsync(request.IdOferta, userId, ct);
        return resultado.Exito ? Ok(resultado) : Conflict(resultado);
    }

    [HttpPost("{idReserva:int}/cancelar")]
    public async Task<IActionResult> Cancelar(int idReserva, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
        var resultado = await _cupos.CancelarAsync(idReserva, userId, ct);
        return resultado.Exito ? Ok(resultado) : Conflict(resultado);
    }
}
