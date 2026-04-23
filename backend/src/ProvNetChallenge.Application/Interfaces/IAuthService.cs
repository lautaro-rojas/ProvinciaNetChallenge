using ProvNetChallenge.Application.DTOs;

namespace ProvNetChallenge.Application.Interfaces
{
    public interface IAuthService
    {
        Task<int> RegisterAsync(UserCreationDto dto);

        Task<UserDto?> LoginAsync(LoginDto dto);
    }
}