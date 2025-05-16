using EventManagement.Application.Interfaces;
using EventManagement.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagement.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetAll()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            Console.WriteLine("Authorization Header recibido: " + authHeader);

            var events = await _eventService.GetAllAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetById(int id)
        {
            var ev = await _eventService.GetByIdAsync(id);
            if (ev == null) return NotFound();
            return Ok(ev);
        }

        [HttpPost]
        public async Task<ActionResult<Event>> Create(Event newEvent)
        {
            var created = await _eventService.AddAsync(newEvent);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Event updatedEvent)
        {
            if (id != updatedEvent.Id)
                return BadRequest("ID mismatch");

            var result = await _eventService.UpdateAsync(updatedEvent);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _eventService.DeleteAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}


