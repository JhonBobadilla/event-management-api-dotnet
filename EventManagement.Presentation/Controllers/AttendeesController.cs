using EventManagement.Application.Dtos;
using EventManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EventManagement.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AttendeesController : ControllerBase
    {
        private readonly IAttendeeService _attendeeService;

        public AttendeesController(IAttendeeService attendeeService)
        {
            _attendeeService = attendeeService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AttendeeRegisterDto dto)
        {
            var result = await _attendeeService.RegisterAsync(dto);
            if (result)
                return Ok(new { message = "Asistente registrado correctamente." });
            else
                return BadRequest("No se pudo registrar el asistente.");
        }
    }
}
