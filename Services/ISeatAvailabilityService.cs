namespace TecnoEdu.Services;

public record ReservationResult(bool Success, string InquiryMessage, int? ReservationId = null);

public interface ISeatAvailabilityService
{
    Task<int?> GetAvailableSeatsAsync(int offeringId, CancellationToken ct = default);
    Task<ReservationResult> ReserveAsync(int offeringId, string userId,
        CancellationToken ct = default);
    Task<ReservationResult> CancelAsync(int reservationId, string userId,
        CancellationToken ct = default);
}
