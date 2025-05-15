namespace EventManagement.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        // El usuario puede crear varios eventos
        public ICollection<Event>? Events { get; set; }
    }
}
