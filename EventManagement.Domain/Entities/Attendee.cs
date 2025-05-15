namespace EventManagement.Domain.Entities
{
    /// Representa a un asistente registrado para un evento,
    /// puede ser usuario de la plataforma o un invitado externo.
        public class Attendee
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? Address { get; set; }
        public DateTime RegisteredAt { get; set; }
        public int EventId { get; set; }
        public int? UserId { get; set; }

        public Event? Event { get; set; }
        public User? User { get; set; }
    }
}
