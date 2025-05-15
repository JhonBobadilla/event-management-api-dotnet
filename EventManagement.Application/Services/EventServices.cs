using EventManagement.Application.Interfaces;
using EventManagement.Domain.Entities;
using EventManagement.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagement.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _eventRepository.GetAllAsync();
        }

        public async Task<Event?> GetByIdAsync(int id)
        {
            return await _eventRepository.GetByIdAsync(id);
        }

        public async Task<Event> AddAsync(Event newEvent)
        {
            return await _eventRepository.AddAsync(newEvent);
        }

        public async Task<bool> UpdateAsync(Event updatedEvent)
        {
            return await _eventRepository.UpdateAsync(updatedEvent);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _eventRepository.DeleteAsync(id);
        }
    }
}
