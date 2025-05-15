namespace EventManagement.Domain.Entities
{
    /// Representa un evento creado dentro de la plataforma.
        public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Location { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public User? Creator { get; set; }
        public ICollection<Attendee> Attendees { get; set; }
    }
}
