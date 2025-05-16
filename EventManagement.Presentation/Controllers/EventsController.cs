using Microsoft.AspNetCore.Http;      // Para IFormFile
using OfficeOpenXml;                 // Para procesar archivos Excel con EPPlus
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

        } 

    } 
 