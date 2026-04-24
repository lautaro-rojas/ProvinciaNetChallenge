using ProvNetChallenge.Application.DTOs;

namespace ProvNetChallenge.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    }
}