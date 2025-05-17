namespace EventManagement.Application.Dtos
{
    public class AttendeeRegisterDto
    {
        public int EventId { get; set; }   // id del evento al que se registra el asistente
        public string Name { get; set; }
        public string Email { get; set; }
        // Agrega más campos si tu entidad Attendee los necesita (Phone, etc.)
    }
}
