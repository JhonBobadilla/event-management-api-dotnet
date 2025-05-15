namespace EventManagement.Domain.Entities
{
    
    /// Representa a un asistente registrado a un evento.
    /// Puede ser un usuario registrado o un invitado externo.
    
    public class Attendee
    {
        public int Id { get; set; }
        
        // Si es usuario registrado, su Id de usuario
        public int? UserId { get; set; }
        public User? User { get; set; }
        
        // Información para asistentes externos (no registrados)
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;
        public DateTime RegisteredAt { get; set; }
    }
}

