namespace EventManagement.Application.Dtos
{
    public class EventSummaryDto
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int AttendeesCount { get; set; }
    }
}

