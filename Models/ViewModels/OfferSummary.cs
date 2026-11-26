namespace TecnoEdu.Models.ViewModels
{
    public class OfferSummary
    {
        public int OfferId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Schedule { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Currency { get; set; } = string.Empty;
        public int AvailableSeats { get; set; }
    }
}
