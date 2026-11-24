using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TecnoEdu.Data;
using TecnoEdu.Models.Entidades;

namespace TecnoEdu.Services;

public class DisponibilidadCuposService : IDisponibilidadCuposService
{
    private readonly TecnoEduDbContext _db;

    public DisponibilidadCuposService(TecnoEduDbContext db) => _db = db;

    public async Task<int?> ConsultarDisponiblesAsync(int idOferta, CancellationToken ct = default)
    {
        var oferta = await _db.OfertasCurso.AsNoTracking()
            .Where(o => o.IdOferta == idOferta)
            .Select(o => new { o.CapacidadTotal, o.Estado })
            .SingleOrDefaultAsync(ct);

        if (oferta is null || oferta.Estado != "Abierta") return null;

        var confirmadas = await _db.Reservas.AsNoTracking().CountAsync(
            r => r.IdOferta == idOferta && r.Estado == "Confirmada", ct);
        return Math.Max(0, oferta.CapacidadTotal - confirmadas);
    }

    public async Task<ResultadoReserva> ReservarAsync(int idOferta, string userId,
        CancellationToken ct = default)
    {
        if (idOferta <= 0 || string.IsNullOrWhiteSpace(userId))
            return new(false, "Oferta o usuario no válido.");

        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        // El bloqueo exclusivo por oferta serializa todas las reservas/cancelaciones
        // que pasen por este servicio, incluso desde distintas instancias del sitio.
        if (!await BloquearOfertaAsync(idOferta, ct))
            return new(false, "La oferta está ocupada. Vuelve a intentar.");

        var oferta = await _db.OfertasCurso.AsNoTracking()
            .Where(o => o.IdOferta == idOferta)
            .Select(o => new { o.IdOferta, o.CapacidadTotal, o.Estado })
            .SingleOrDefaultAsync(ct);

        if (oferta is null || oferta.Estado != "Abierta")
            return new(false, "La oferta no está abierta para reservas.");

        if (await _db.Reservas.AnyAsync(r =>
            r.IdOferta == idOferta && r.UserId == userId &&
            r.Estado == "Confirmada", ct))
            return new(false, "Ya tienes una reserva confirmada para esta oferta.");

        var confirmadas = await _db.Reservas.CountAsync(
            r => r.IdOferta == idOferta && r.Estado == "Confirmada", ct);
        if (confirmadas >= oferta.CapacidadTotal)
            return new(false, "No quedan cupos disponibles.");

        var reserva = new Reserva
        {
            IdOferta = idOferta,
            UserId = userId,
            FechaSolicitud = DateTime.UtcNow,
            Estado = "Confirmada"
        };
        _db.Reservas.Add(reserva);

        try
        {
            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
            return new(true, "Reserva confirmada.", reserva.IdReserva);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException { Number: 2601 or 2627 })
        {
            await tx.RollbackAsync(ct);
            return new(false, "Ya existe una reserva confirmada de este usuario.");
        }
    }

    public async Task<ResultadoReserva> CancelarAsync(int idReserva, string userId,
        CancellationToken ct = default)
    {
        if (idReserva <= 0 || string.IsNullOrWhiteSpace(userId))
            return new(false, "Reserva o usuario no válido.");

        var referencia = await _db.Reservas.AsNoTracking()
            .Where(r => r.IdReserva == idReserva && r.UserId == userId)
            .Select(r => new { r.IdOferta })
            .SingleOrDefaultAsync(ct);
        if (referencia is null)
            return new(false, "No existe una reserva de este usuario.");

        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        if (!await BloquearOfertaAsync(referencia.IdOferta, ct))
            return new(false, "La oferta está ocupada. Vuelve a intentar.");

        var reserva = await _db.Reservas.SingleOrDefaultAsync(r =>
            r.IdReserva == idReserva && r.UserId == userId, ct);

        if (reserva is null || reserva.Estado != "Confirmada")
            return new(false, "La reserva no está confirmada o ya fue cancelada.");

        reserva.Estado = "Cancelada";
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
        return new(true, "Reserva cancelada.", reserva.IdReserva);
    }

    private async Task<bool> BloquearOfertaAsync(int idOferta, CancellationToken ct)
    {
        // SQL Server sp_getapplock exige una transacción abierta con LockOwner=Transaction.
        // Resultado >= 0: bloqueo concedido; negativo: timeout/error/deadlock.
        var resultado = new SqlParameter("@resultado", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        var recurso = new SqlParameter("@recurso", SqlDbType.NVarChar, 255)
        {
            Value = $"TecnoEdu:Oferta:{idOferta}"
        };

        await _db.Database.ExecuteSqlRawAsync(
            "EXEC @resultado = sys.sp_getapplock " +
            "@Resource = @recurso, @LockMode = 'Exclusive', " +
            "@LockOwner = 'Transaction', @LockTimeout = 10000",
            new object[] { resultado, recurso }, ct);

        return resultado.Value is not DBNull &&
            resultado.Value is not null && Convert.ToInt32(resultado.Value) >= 0;
    }
}
