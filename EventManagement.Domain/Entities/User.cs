namespace EventManagement.Domain.Entities
{
    // Representa a un usuario registrado en la plataforma,
    // capaz de crear eventos y también inscribirse como asistente.
    public class User
    {
        /// Identificador único del usuario (Primary Key). Y demás propiedades.
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string City { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<Attendee> Attendances { get; set; }
    }
}


