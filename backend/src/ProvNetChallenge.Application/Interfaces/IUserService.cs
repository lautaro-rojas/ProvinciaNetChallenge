using ProvNetChallenge.Application.DTOs;

namespace ProvNetChallenge.Application.Interfaces
{
    public interface IUserService
    {
        // Obtener todos los usuarios (devuelve DTOs, no Entidades)
        Task<List<UserDto>> GetAllAsync();

        // Obtener uno por ID
        Task<UserDto?> GetByIdAsync(int id);

        // Crear un nuevo usuario (Recibe el DTO de creación, devuelve el ID generado)
        Task<int> AddAsync(UserCreationDto dto);

        // Actualizar (Recibe DTO, devuelve true si funcionó)
        Task<bool> UpdateAsync(int id, UserUpdateDto dto);

        // Borrado físico
        Task<bool> DeleteAsync(int id);
        
        // Borrado lógico
        Task<bool> DeleteLogicAsync(int id, UserCreationDto dto);
    }
}