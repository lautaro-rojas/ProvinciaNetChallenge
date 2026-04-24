using ProvNetChallenge.Domain.Entities;
using ProvNetChallenge.Application.Interfaces;
using ProvNetChallenge.Application.DTOs;
using ProvNetChallenge.Application.Interfaces.Repositories;

namespace ProvNetChallenge.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            // Mapeo manual de Entidad a DTO. 
            // En un proyecto gigante usaríamos AutoMapper, pero para 48hs el mapeo manual 
            // es más rápido, consume menos memoria y demuestra que entendés lo que pasa por detrás.
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
                Email = u.Email,
                IsActive = u.IsActive,
                DateActivation = u.DateActivation,
                DateModification = u.DateModification,
                DateDeactivation = u.DateDeactivation
            }).ToList();
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                IsActive = user.IsActive,
                DateActivation = user.DateActivation,
                DateModification = user.DateModification,
                DateDeactivation = user.DateDeactivation
            };
        }

        public async Task<int> AddAsync(UserCreationDto dto)
        {
            // REGLA DE NEGOCIO SR: Idealmente acá deberías verificar si el email ya existe
            // llamando a un _userRepository.GetByEmailAsync(dto.Email). 
            // Si existe, lanzarías una Custom Exception (ej. BadRequestException).

            // Encriptamos la contraseña (Nunca se guarda en texto plano en una BD financiera)
            // Tip: Para que esto funcione, instalá el paquete NuGet "BCrypt.Net-Next" en la capa Application.
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var newUser = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                Email = dto.Email,
                PasswordHash = passwordHash, 
                DateActivation = DateTime.UtcNow,
                IsActive = true
            };

            await _userRepository.AddAsync(newUser);
            
            return newUser.Id; 
        }

        public async Task<bool> UpdateAsync(int id, UserUpdateDto dto)
        {
            // Buscamos si existe
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null) return false;

            // Modificamos solo lo que permitimos cambiar
            existingUser.FirstName = dto.FirstName;
            existingUser.LastName = dto.LastName;
            // No permitimos cambiar ni el Email ni el Password por este endpoint por seguridad

            await _userRepository.UpdateAsync(existingUser);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            await _userRepository.DeleteAsync(user);
            return true;
        }

        public async Task<bool> DeleteLogicAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            // REGLA DE NEGOCIO: Borrado lógico.
            // Cambiamos el estado, pero la data financiera o histórica queda intacta.
            user.IsActive = false;

            await _userRepository.UpdateAsync(user);
            return true;
        }

    }
}