namespace TMS.Api.Models.Dto
{
    public class OrderPatchDto
    {
        public int OrderId { get; set; }

        public DateTime OrderedAt { get; set; }

        public int NumberOfTickets { get; set; }

        public decimal? TotalPrice { get; set; }
    }
}
