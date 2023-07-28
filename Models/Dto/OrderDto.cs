namespace TMS.Api.Models.Dto
{
    public class OrderDto
    {
        public int OrderId { get; set; }

        public DateTime OrderedAt { get; set; }

        public int NumberOfTickets { get; set; }

        public decimal? TotalPrice { get; set; }
        public string Customer { get; set; }

        public string TicketCategory { get; set; }
    }
}
