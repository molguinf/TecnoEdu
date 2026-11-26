using Microsoft.AspNetCore.Mvc;
using TecnoEdu.Services;

namespace TecnoEdu.Controllers
{
    // JSON API: GET api/offers/5/seats
    [ApiController]
    [Route("api/offers")]
    public class OffersController : ControllerBase
    {
        private readonly IDisponibilidadCuposService _seatService;

        public OffersController(IDisponibilidadCuposService seatService)
        {
            _seatService = seatService;
        }

        [HttpGet("{offerId:int}/seats")]
        public async Task<IActionResult> Seats(int offerId)
        {
            var availableSeats = await _seatService.ConsultarDisponiblesAsync(offerId);
            if (availableSeats == null)
            {
                return NotFound(new { message = "Offer not found or closed." });
            }

            return Ok(new { offerId, availableSeats });
        }
    }
}
