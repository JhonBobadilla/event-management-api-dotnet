namespace EventManagement.Domain.Entities
{
    
    /// Representa un usuario registrado en la plataforma.
    
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!; // Guarda la contraseña en hash para seguridad
        public string Phone { get; set; } = null!;
        public string City { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public ICollection<Event> EventsCreated { get; set; } = new List<Event>(); // Eventos creados por el usuario
        public ICollection<Attendee> AttendedEvents { get; set; } = new List<Attendee>(); // Eventos a los que asiste

    }
}



