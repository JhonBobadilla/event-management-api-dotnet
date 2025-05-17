using EventManagement.Domain.Entities;
using EventManagement.Infrastructure.Data;
using EventManagement.Infrastructure.Interfaces;
using System.Threading.Tasks;

namespace EventManagement.Infrastructure.Repositories
{
    public class AttendeeRepository : IAttendeeRepository
    {
        private readonly ApplicationDbContext _context;

        public AttendeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Attendee attendee)
        {
            await _context.Attendees.AddAsync(attendee);
            await _context.SaveChangesAsync();
        }
    }
}
