namespace TecnoEdu.Services;

public record ReservationResult(bool Success, string InquiryMessage, int? ReservationId = null);

public interface ISeatAvailabilityService
{
    Task<int?> GetAvailableSeatsAsync(int idOferta, CancellationToken ct = default);
    Task<ReservationResult> ReserveAsync(int idOferta, string userId,
        CancellationToken ct = default);
    Task<ReservationResult> CancelAsync(int idReserva, string userId,
        CancellationToken ct = default);
}
