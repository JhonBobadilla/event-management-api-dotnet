using EventManagement.Application.Interfaces;
using EventManagement.Application.Dtos; 
using EventManagement.Domain.Entities;
using EventManagement.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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

        // MÉTODO CORRECTO PARA AGREGAR VARIOS EVENTOS EN UN SOLO LLAMADO A LA BD
        public async Task AddRangeAsync(IEnumerable<Event> events)
        {
            await _eventRepository.AddRangeAsync(events);
        }

        // === NUEVO MÉTODO ===
        public async Task<Dictionary<string, DaySummaryDto>> GetAttendeesSummaryByDayOfWeekAsync()
        {
            // Usando el repositorio para obtener los eventos CON asistentes
            var eventsWithAttendees = await _eventRepository.GetAllWithAttendeesAsync();

            var result = new Dictionary<string, DaySummaryDto>
            {
                { "Monday", new DaySummaryDto() },
                { "Tuesday", new DaySummaryDto() },
                { "Wednesday", new DaySummaryDto() },
                { "Thursday", new DaySummaryDto() },
                { "Friday", new DaySummaryDto() },
                { "Saturday", new DaySummaryDto() },
                { "Sunday", new DaySummaryDto() },
            };

            foreach (var ev in eventsWithAttendees)
            {
                // Usa el nombre del día en inglés, puedes traducirlo si quieres
                var dayOfWeek = ev.Date.DayOfWeek.ToString();

                if (result.ContainsKey(dayOfWeek))
                {
                    var summary = result[dayOfWeek];
                    summary.TotalEvents++;
                    int attendeeCount = ev.Attendees?.Count ?? 0;
                    summary.TotalAttendees += attendeeCount;
                    summary.Events.Add(new EventSummaryDto
                    {
                        EventId = ev.Id,
                        Title = ev.Title,
                        Date = ev.Date,
                        AttendeesCount = attendeeCount
                    });
                }
            }

            return result;
        }
    }
}


