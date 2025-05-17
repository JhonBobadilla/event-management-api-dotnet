using EventManagement.Application.Dtos;
using EventManagement.Domain.Entities;
using EventManagement.Infrastructure.Interfaces;
using System.Threading.Tasks;
using EventManagement.Application.Interfaces;


namespace EventManagement.Application.Services
{
    public class AttendeeService : IAttendeeService
    {
        private readonly IAttendeeRepository _attendeeRepository;

        public AttendeeService(IAttendeeRepository attendeeRepository)
        {
            _attendeeRepository = attendeeRepository;
        }

        public async Task<bool> RegisterAsync(AttendeeRegisterDto dto)
        {
            var attendee = new Attendee
            {
                EventId = dto.EventId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                City = dto.City,
                Address = dto.Address, 
                RegisteredAt = DateTime.UtcNow
            };  
            await _attendeeRepository.AddAsync(attendee);
            return true;
        }
    }
}
