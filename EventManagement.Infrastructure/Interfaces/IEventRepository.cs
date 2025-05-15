//Este archivo describe qué métodos debe tener cualquier clase que maneje la persistencia de eventos.

using EventManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagement.Infrastructure.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(int id);
        Task<Event> AddAsync(Event entity);
        Task<bool> UpdateAsync(Event entity);
        Task<bool> DeleteAsync(int id);
    }
}
