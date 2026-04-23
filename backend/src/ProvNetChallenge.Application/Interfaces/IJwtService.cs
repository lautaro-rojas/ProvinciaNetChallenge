namespace ProvNetChallenge.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(string userId, string email, string userName);
    }
}