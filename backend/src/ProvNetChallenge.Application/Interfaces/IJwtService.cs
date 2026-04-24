using ProvNetChallenge.Domain.Entities;

namespace ProvNetChallenge.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
    }
}