using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TecnoEdu.Data;
using TecnoEdu.Models.Entities;

namespace TecnoEdu.Services;

public class SeatAvailabilityService : ISeatAvailabilityService
{
    private readonly TecnoEduDbContext _db;

    public SeatAvailabilityService(TecnoEduDbContext db) => _db = db;

    public async Task<int?> GetAvailableSeatsAsync(int idOferta, CancellationToken ct = default)
    {
        var oferta = await _db.CourseOfferings.AsNoTracking()
            .Where(o => o.OfferingId == idOferta)
            .Select(o => new { o.TotalCapacity, o.Status })
            .SingleOrDefaultAsync(ct);

        if (oferta is null || oferta.Status != "Open") return null;

        var confirmadas = await _db.Reservations.AsNoTracking().CountAsync(
            r => r.OfferingId == idOferta && r.Status == "Confirmed", ct);
        return Math.Max(0, oferta.TotalCapacity - confirmadas);
    }

    public async Task<ReservationResult> ReserveAsync(int idOferta, string userId,
        CancellationToken ct = default)
    {
        if (idOferta <= 0 || string.IsNullOrWhiteSpace(userId))
            return new(false, "Oferta o usuario no válido.");

        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        // El bloqueo exclusivo por oferta serializa todas las reservas/cancelaciones
        // que pasen por este servicio, incluso desde distintas instancias del sitio.
        if (!await LockOfferingAsync(idOferta, ct))
            return new(false, "La oferta está ocupada. Vuelve a intentar.");

        var oferta = await _db.CourseOfferings.AsNoTracking()
            .Where(o => o.OfferingId == idOferta)
            .Select(o => new { o.OfferingId, o.TotalCapacity, o.Status })
            .SingleOrDefaultAsync(ct);

        if (oferta is null || oferta.Status != "Open")
            return new(false, "La oferta no está abierta para reservas.");

        if (await _db.Reservations.AnyAsync(r =>
            r.OfferingId == idOferta && r.UserId == userId &&
            r.Status == "Confirmed", ct))
            return new(false, "Ya tienes una reserva confirmada para esta oferta.");

        var confirmadas = await _db.Reservations.CountAsync(
            r => r.OfferingId == idOferta && r.Status == "Confirmed", ct);
        if (confirmadas >= oferta.TotalCapacity)
            return new(false, "No quedan cupos disponibles.");

        var reserva = new Reservation
        {
            OfferingId = idOferta,
            UserId = userId,
            RequestedAt = DateTime.UtcNow,
            Status = "Confirmed"
        };
        _db.Reservations.Add(reserva);

        try
        {
            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
            return new(true, "Reservation confirmada.", reserva.ReservationId);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException { Number: 2601 or 2627 })
        {
            await tx.RollbackAsync(ct);
            return new(false, "Ya existe una reserva confirmada de este usuario.");
        }
    }

    public async Task<ReservationResult> CancelAsync(int idReserva, string userId,
        CancellationToken ct = default)
    {
        if (idReserva <= 0 || string.IsNullOrWhiteSpace(userId))
            return new(false, "Reservation o usuario no válido.");

        var referencia = await _db.Reservations.AsNoTracking()
            .Where(r => r.ReservationId == idReserva && r.UserId == userId)
            .Select(r => new { r.OfferingId })
            .SingleOrDefaultAsync(ct);
        if (referencia is null)
            return new(false, "No existe una reserva de este usuario.");

        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        if (!await LockOfferingAsync(referencia.OfferingId, ct))
            return new(false, "La oferta está ocupada. Vuelve a intentar.");

        var reserva = await _db.Reservations.SingleOrDefaultAsync(r =>
            r.ReservationId == idReserva && r.UserId == userId, ct);

        if (reserva is null || reserva.Status != "Confirmed")
            return new(false, "La reserva no está confirmada o ya fue cancelada.");

        reserva.Status = "Cancelled";
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
        return new(true, "Reservation cancelada.", reserva.ReservationId);
    }

    private async Task<bool> LockOfferingAsync(int idOferta, CancellationToken ct)
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
