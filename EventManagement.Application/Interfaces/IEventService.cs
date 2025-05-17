using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagement.Domain.Entities;
using EventManagement.Application.Dtos;

namespace EventManagement.Application.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(int id);
        Task<Event> AddAsync(Event newEvent);
        Task<bool> UpdateAsync(Event updatedEvent);
        Task<bool> DeleteAsync(int id);
        Task AddRangeAsync(IEnumerable<Event> events);
        Task<Dictionary<string, DaySummaryDto>> GetAttendeesSummaryByDayOfWeekAsync();
    }
}

