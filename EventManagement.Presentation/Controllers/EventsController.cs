using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using EventManagement.Application.Interfaces;
using EventManagement.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagement.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using EventManagement.Application.Dtos; // <-- IMPORTANTE para los DTOs

namespace EventManagement.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly MapboxNearbyService _mapboxService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EventsController> _logger;

        public EventsController(
            IEventService eventService,
            MapboxNearbyService mapboxService,
            IConfiguration configuration,
            ILogger<EventsController> logger)
        {
            _eventService = eventService;
            _mapboxService = mapboxService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("{id}/nearby")]
        public async Task<IActionResult> GetNearbyPlaces(int id, [FromQuery] int radius = 500)
        {
            try
            {
                // 1. Obtener el evento
                var evento = await _eventService.GetByIdAsync(id);
                if (evento == null)
                    return NotFound("Evento no encontrado.");

                // 2. Validar coordenadas
                if (!evento.Latitude.HasValue || !evento.Longitude.HasValue)
                    return BadRequest("El evento no tiene coordenadas registradas.");

                // 3. Validar radio
                if (radius <= 0 || radius > 10000) // Máximo 10km
                    return BadRequest("El radio debe estar entre 1 y 10000 metros");

                // 4. Obtener lugares cercanos
                var places = await _mapboxService.GetNearbyPlacesAsync(
                    evento.Latitude.Value,
                    evento.Longitude.Value,
                    radius
                );

                return Ok(places);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener lugares cercanos para el evento {id}");
                return StatusCode(500, "Error interno al buscar lugares cercanos");
            }
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
            // 1. Obtener el ID del usuario autenticado desde el JWT
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "nameid" || c.Type.EndsWith("nameidentifier"));
            if (userIdClaim == null)
                return Unauthorized("No se pudo identificar el usuario.");

            // 2. Convertir el claim a int
            int creatorId;
            if (!int.TryParse(userIdClaim.Value, out creatorId))
                return BadRequest("El identificador del usuario es inválido.");

            // 3. Asignar el CreatorId al evento
            newEvent.CreatedBy = creatorId;
            newEvent.Date = DateTime.SpecifyKind(newEvent.Date, DateTimeKind.Utc);

            // 4. Guardar el evento
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

        [HttpPost("upload-excel")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (file == null || file.Length == 0)
                return BadRequest("Debe adjuntar un archivo Excel.");

            // Obtener el ID del usuario autenticado desde el JWT
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "nameid" || c.Type.EndsWith("nameidentifier"));
            if (userIdClaim == null)
                return Unauthorized("No se pudo identificar el usuario.");

            int creatorId;
            if (!int.TryParse(userIdClaim.Value, out creatorId))
                return BadRequest("El identificador del usuario es inválido.");

            var eventsList = new List<Event>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);

                using (var package = new OfficeOpenXml.ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                        return BadRequest("El archivo no tiene hojas.");

                    int rowCount = worksheet.Dimension.Rows;
                    // Empieza en la fila 2 (saltando el encabezado)
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var titleCell = worksheet.Cells[row, 1]?.Text;
                        if (string.IsNullOrWhiteSpace(titleCell)) continue;

                        double lat = 0;
                        double lng = 0;
                        double.TryParse(worksheet.Cells[row, 7].Text, out lat);
                        double.TryParse(worksheet.Cells[row, 8].Text, out lng);

                        var newEvent = new Event
                        {
                            Title = worksheet.Cells[row, 1].Text,
                            Description = worksheet.Cells[row, 2].Text,
                            Date = DateTime.TryParse(worksheet.Cells[row, 3].Text, out var dt) ? DateTime.SpecifyKind(dt, DateTimeKind.Utc) : DateTime.UtcNow,
                            Location = worksheet.Cells[row, 4].Text,
                            Address = worksheet.Cells[row, 5].Text,
                            City = worksheet.Cells[row, 6].Text,
                            Latitude = (float)lat,
                            Longitude = (float)lng,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = creatorId
                        };
                        eventsList.Add(newEvent);
                    }
                }
            }

            if (eventsList.Count == 0)
                return BadRequest("No se encontraron eventos válidos en el archivo.");

            await _eventService.AddRangeAsync(eventsList);

            return Ok(new { message = $"Se registraron {eventsList.Count} eventos correctamente." });
        }

        // === NUEVO ENDPOINT: Análisis avanzado de asistentes ===
        [HttpGet("attendees-by-day")]
        public async Task<IActionResult> GetAttendeesSummaryByDayOfWeek()
        {
            var result = await _eventService.GetAttendeesSummaryByDayOfWeekAsync();
            return Ok(result);
        }
    }
}
