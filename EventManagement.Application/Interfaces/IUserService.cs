using System.Threading.Tasks;
using EventManagement.Application.DTOs;

namespace EventManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(RegisterUserDto dto);
    }
}
