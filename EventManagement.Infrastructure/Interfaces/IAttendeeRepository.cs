using EventManagement.Domain.Entities;
using System.Threading.Tasks;

namespace EventManagement.Infrastructure.Interfaces
{
    public interface IAttendeeRepository
    {
        Task AddAsync(Attendee attendee);
        
    }
}
