namespace EventManagement.Domain.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Location { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Foreign Key - usuario que creó el evento
        public int CreatedBy { get; set; }
        public User? User { get; set; }

        // Relación: un evento puede tener muchos asistentes
        public ICollection<Attendee>? Attendees { get; set; }
    }
}
