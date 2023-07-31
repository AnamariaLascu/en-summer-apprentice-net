namespace TMS.Api.Models.Dto
{
    public class EventTypeDto
    {
        public int EventTypeId { get; set; }
        public string EventTypeName { get; set; }
        public ICollection<Event>? Events { get; set; }
    }
}
