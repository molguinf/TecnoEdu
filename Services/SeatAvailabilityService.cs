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

    public async Task<int?> GetAvailableSeatsAsync(int offeringId, CancellationToken ct = default)
    {
        var offering = await _db.CourseOfferings.AsNoTracking()
            .Where(o => o.OfferingId == offeringId)
            .Select(o => new { o.TotalCapacity, o.Status })
            .SingleOrDefaultAsync(ct);

        if (offering is null || offering.Status != "Open") return null;

        var confirmedCount = await _db.Reservations.AsNoTracking().CountAsync(
            r => r.OfferingId == offeringId && r.Status == "Confirmed", ct);
        return Math.Max(0, offering.TotalCapacity - confirmedCount);
    }

    public async Task<ReservationResult> ReserveAsync(int offeringId, string userId,
        CancellationToken ct = default)
    {
        if (offeringId <= 0 || string.IsNullOrWhiteSpace(userId))
            return new(false, "Oferta o usuario no válido.");

        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        // El bloqueo exclusivo por oferta serializa todas las reservas/cancelaciones
        // que pasen por este servicio, incluso desde distintas instancias del sitio.
        if (!await LockOfferingAsync(offeringId, ct))
            return new(false, "La oferta está ocupada. Vuelve a intentar.");

        var offering = await _db.CourseOfferings.AsNoTracking()
            .Where(o => o.OfferingId == offeringId)
            .Select(o => new { o.OfferingId, o.TotalCapacity, o.Status })
            .SingleOrDefaultAsync(ct);

        if (offering is null || offering.Status != "Open")
            return new(false, "La oferta no está abierta para reservas.");

        if (await _db.Reservations.AnyAsync(r =>
            r.OfferingId == offeringId && r.UserId == userId &&
            r.Status == "Confirmed", ct))
            return new(false, "Ya tienes una reserva confirmada para esta offering.");

        var confirmedCount = await _db.Reservations.CountAsync(
            r => r.OfferingId == offeringId && r.Status == "Confirmed", ct);
        if (confirmedCount >= offering.TotalCapacity)
            return new(false, "No quedan cupos disponibles.");

        var reservation = new Reservation
        {
            OfferingId = offeringId,
            UserId = userId,
            RequestedAt = DateTime.UtcNow,
            Status = "Confirmed"
        };
        _db.Reservations.Add(reservation);

        try
        {
            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
            return new(true, "Reservation confirmada.", reservation.ReservationId);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException { Number: 2601 or 2627 })
        {
            await tx.RollbackAsync(ct);
            return new(false, "Ya existe una reserva confirmada de este usuario.");
        }
    }

    public async Task<ReservationResult> CancelAsync(int reservationId, string userId,
        CancellationToken ct = default)
    {
        if (reservationId <= 0 || string.IsNullOrWhiteSpace(userId))
            return new(false, "Reservation o usuario no válido.");

        var reservationReference = await _db.Reservations.AsNoTracking()
            .Where(r => r.ReservationId == reservationId && r.UserId == userId)
            .Select(r => new { r.OfferingId })
            .SingleOrDefaultAsync(ct);
        if (reservationReference is null)
            return new(false, "No existe una reserva de este usuario.");

        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        if (!await LockOfferingAsync(reservationReference.OfferingId, ct))
            return new(false, "La oferta está ocupada. Vuelve a intentar.");

        var reservation = await _db.Reservations.SingleOrDefaultAsync(r =>
            r.ReservationId == reservationId && r.UserId == userId, ct);

        if (reservation is null || reservation.Status != "Confirmed")
            return new(false, "La reserva no está confirmada o ya fue cancelada.");

        reservation.Status = "Cancelled";
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
        return new(true, "Reservation cancelada.", reservation.ReservationId);
    }

    private async Task<bool> LockOfferingAsync(int offeringId, CancellationToken ct)
    {
        // SQL Server sp_getapplock exige una transacción abierta con LockOwner=Transaction.
        // Result >= 0: lock acquired; negative: timeout/error/deadlock.
        var lockResult = new SqlParameter("@lockResult", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        var lockResource = new SqlParameter("@lockResource", SqlDbType.NVarChar, 255)
        {
            Value = $"TecnoEdu:Offering:{offeringId}"
        };

        await _db.Database.ExecuteSqlRawAsync(
            "EXEC @lockResult = sys.sp_getapplock " +
            "@Resource = @lockResource, @LockMode = 'Exclusive', " +
            "@LockOwner = 'Transaction', @LockTimeout = 10000",
            new object[] { lockResult, lockResource }, ct);

        return lockResult.Value is not DBNull &&
            lockResult.Value is not null && Convert.ToInt32(lockResult.Value) >= 0;
    }
}
