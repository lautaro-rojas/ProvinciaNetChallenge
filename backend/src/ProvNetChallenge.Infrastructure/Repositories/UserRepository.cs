using ProvNetChallenge.Application.DTOs;
using ProvNetChallenge.Application.Interfaces.Repositories;
using ProvNetChallenge.Infrastructure.Data;
using ProvNetChallenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProvNetChallenge.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            // Ojo al nivel Senior: Filtramos por IsActive para respetar el borrado lógico.
            return await _dbContext.USER
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            // Usamos FirstOrDefaultAsync en lugar de FindAsync porque necesitamos 
            // agregar la condición del IsActive.
            return await _dbContext.USER
                .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
        }
        
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.USER
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
        }

        public async Task AddAsync(User user)
        {
            await _dbContext.USER.AddAsync(user);
            await _dbContext.SaveChangesAsync(); // Impacta en SQL Server
        }

        public async Task UpdateAsync(User user)
        {
            // EF Core trackea los cambios, Update marca la entidad como modificada
            _dbContext.USER.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            // Esto es un borrado FÍSICO (DELETE en SQL). 
            // Aunque el challenge use borrado lógico, siempre es buena práctica 
            // dejar el borrado físico disponible en el repositorio por si la base 
            // de datos necesita mantenimiento o limpieza por GDPR.
            _dbContext.USER.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

    }
}
/* 
    Para este MVP de 48hs se optó por ejecutar SaveChanges() directamente en el Repositorio. 
    Para escalar la solución a futuro, se podría implementar un patrón Unit of Work para manejar transacciones más complejas.
*/