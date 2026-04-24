using ProvNetChallenge.Application.DTOs;
using ProvNetChallenge.Application.Interfaces.Repositories;
using ProvNetChallenge.Infrastructure.Data;
using ProvNetChallenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProvNetChallenge.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            // Ojo al nivel Senior: Filtramos por IsActive para respetar el borrado lógico.
            return await _context.USER
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            // Usamos FirstOrDefaultAsync en lugar de FindAsync porque necesitamos 
            // agregar la condición del IsActive.
            return await _context.USER
                .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
        }
        
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.USER
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
        }

        public async Task AddAsync(User user)
        {
            await _context.USER.AddAsync(user);
            await _context.SaveChangesAsync(); // Impacta en SQL Server
        }

        public async Task UpdateAsync(User user)
        {
            // EF Core trackea los cambios, Update marca la entidad como modificada
            _context.USER.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            // Esto es un borrado FÍSICO (DELETE en SQL). 
            // Aunque el challenge use borrado lógico, siempre es buena práctica 
            // dejar el borrado físico disponible en el repositorio por si la base 
            // de datos necesita mantenimiento o limpieza por GDPR.
            _context.USER.Remove(user);
            await _context.SaveChangesAsync();
        }

    }
}
/* 
    Para este MVP de 48hs se optó por ejecutar SaveChanges() directamente en el Repositorio. 
    Para escalar la solución a futuro, se podría implementar un patrón Unit of Work para manejar transacciones más complejas.
*/