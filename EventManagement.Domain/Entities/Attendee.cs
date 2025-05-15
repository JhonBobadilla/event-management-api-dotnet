namespace EventManagement.Domain.Entities
{
    public class Attendee
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime RegisteredAt { get; set; }

        // Foreign Key - evento al que asiste
        public int EventId { get; set; }
        public Event? Event { get; set; }
    }
}
