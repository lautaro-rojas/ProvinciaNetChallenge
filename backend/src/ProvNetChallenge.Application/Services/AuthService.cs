using ProvNetChallenge.Application.DTOs;
using ProvNetChallenge.Application.Interfaces;
using ProvNetChallenge.Application.Interfaces.Repositories;

namespace ProvNetChallenge.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        // Inyectamos el repo (para buscar al user) y el jwt service (para crear el token)
        public AuthService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            // Buscamos al usuario por email
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            
            // Si no existe o fue dado de baja (borrado lógico), denegamos el acceso
            if (user == null || !user.IsActive) 
                return null;

            // Verificamos la contraseña (asumiendo que instalaste BCrypt.Net-Next)
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!isPasswordValid)
                return null;

            // Generamos el token usando la abstracción
            var token = _jwtService.GenerateJwtToken(user);

            // Devolvemos el token junto con algunos datos útiles
            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                UserName = user.UserName
            };
        }
    }
}