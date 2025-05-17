using System.Collections.Generic;

namespace EventManagement.Application.Dtos
{
    public class DaySummaryDto
    {
        public int TotalEvents { get; set; }
        public int TotalAttendees { get; set; }
        public List<EventSummaryDto> Events { get; set; } = new();
    }
}
