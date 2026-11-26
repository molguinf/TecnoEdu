using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TecnoEdu.Models.ViewModels;
using TecnoEdu.Services;

namespace TecnoEdu.Controllers
{
    // JSON API: POST api/reservations  and  POST api/reservations/5/cancel
    [ApiController]
    [Authorize]
    [Route("api/reservations")]
    public class ReservationsController : ControllerBase
    {
        private readonly ISeatAvailabilityService _seatService;

        public ReservationsController(ISeatAvailabilityService seatService)
        {
            _seatService = seatService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var result = await _seatService.ReserveAsync(request.OfferId, userId);
            var response = new { success = result.Success, message = result.InquiryMessage, reservationId = result.ReservationId };
            return result.Success ? Ok(response) : Conflict(response);
        }

        [HttpPost("{reservationId:int}/cancel")]
        public async Task<IActionResult> Cancel(int reservationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var result = await _seatService.CancelAsync(reservationId, userId);
            var response = new { success = result.Success, message = result.InquiryMessage, reservationId = result.ReservationId };
            return result.Success ? Ok(response) : Conflict(response);
        }
    }
}
