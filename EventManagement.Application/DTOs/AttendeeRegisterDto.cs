namespace EventManagement.Application.Dtos
{
    public class AttendeeRegisterDto
{
    public int EventId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public string City { get; set; }
    public string Address { get; set; } 
}
}
