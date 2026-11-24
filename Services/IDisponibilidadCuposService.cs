namespace TecnoEdu.Services;

public record ResultadoReserva(bool Exito, string Mensaje, int? IdReserva = null);

public interface IDisponibilidadCuposService
{
    Task<int?> ConsultarDisponiblesAsync(int idOferta, CancellationToken ct = default);
    Task<ResultadoReserva> ReservarAsync(int idOferta, string userId,
        CancellationToken ct = default);
    Task<ResultadoReserva> CancelarAsync(int idReserva, string userId,
        CancellationToken ct = default);
}
