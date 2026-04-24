using ProvNetChallenge.Domain.Entities;

namespace ProvNetChallenge.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        // Devuelve entidades puras
        Task<IEnumerable<User>> GetAllAsync(); 

        // Devuelve la entidad o null si no la encuentra
        Task<User?> GetByIdAsync(int id);

        Task<User?> GetByEmailAsync(string email);

        // Recibe la entidad para guardarla
        Task AddAsync(User user);

        // Recibe la entidad modificada para actualizarla en la BD
        Task UpdateAsync(User user);

        // Recibe la entidad para eliminarla físicamente
        Task DeleteAsync(User user);
    }
}