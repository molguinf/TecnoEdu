using System.ComponentModel.DataAnnotations;

namespace TecnoEdu.Models.ViewModels
{
    public class CreateReservationRequest
    {
        [Range(1, int.MaxValue)]
        public int OfferId { get; set; }
    }
}
