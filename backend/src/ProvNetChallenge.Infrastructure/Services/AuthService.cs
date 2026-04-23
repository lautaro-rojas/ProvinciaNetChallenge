using ProvNetChallenge.Application.Interfaces;
using ProvNetChallenge.Application.DTOs;
using ProvNetChallenge.Infrastructure.Data;

namespace ProvNetChallenge.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        public AuthService(ApplicationDbContext db) => _db = db;

        public async Task<UserDto?> LoginAsync(LoginDto dto)
        {
            var user = _db.USER.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null) return null;

            // Aquí deberías comparar el password hasheado, pero para el ejemplo lo dejamos simple
            if (user.Password != dto.Password)
                return null;

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                DateActivation = user.DateActivation
            };
        }

        public async Task<int> RegisterAsync(UserCreationDto dto)
        {
            throw new NotImplementedException();
        }
    }
}