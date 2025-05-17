using System.Threading.Tasks;
using EventManagement.Application.Dtos;


namespace EventManagement.Application.Interfaces
{
    public interface IAttendeeService
    {
        Task<bool> RegisterAsync(AttendeeRegisterDto dto);
       
    }
}
