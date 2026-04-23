using Microsoft.EntityFrameworkCore;
using ProvNetChallenge.Domain.Entities;
using ProvNetChallenge.Application.Interfaces;
using ProvNetChallenge.Application.DTOs;
using ProvNetChallenge.Infrastructure.Data;

namespace ProvNetChallenge.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _db;
        public UserService(ApplicationDbContext db) => _db = db;

        public async Task<int> AddAsync(UserCreationDto dto)
        {
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                Email = dto.Email,
                Password = dto.Password, // Hash before save
                ActiveAccount = true,
                DateActivation = DateTime.UtcNow,
                DateModification = DateTime.UtcNow,
                DateDeactivation = DateTime.UtcNow
            };

            await _db.USER.AddAsync(user);
            await _db.SaveChangesAsync();
            return user.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _db.USER.FindAsync(id);
            if (user == null) return false;

            _db.USER.Remove(user);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteLogicAsync(int id, UserCreationDto dto)
        {
            var user = await _db.USER.FindAsync(id);
            if (user == null) return false;

            user.ActiveAccount = false;
            user.DateDeactivation = DateTime.UtcNow;
            user.DateModification = DateTime.UtcNow;

            _db.USER.Update(user);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            return await _db.USER
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    UserName = u.UserName,
                    Email = u.Email,
                    ActiveAccount = u.ActiveAccount,
                    DateActivation = u.DateActivation,
                    DateModification = u.DateModification,
                    DateDeactivation = u.DateDeactivation
                })
                .ToListAsync();
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var u = await _db.USER.FindAsync(id);
            if (u == null) return null;

            return new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
                Email = u.Email,
                ActiveAccount = u.ActiveAccount,
                DateActivation = u.DateActivation,
                DateModification = u.DateModification,
                DateDeactivation = u.DateDeactivation
            };
        }

        public async Task<bool> UpdateAsync(int id, UserUpdateDto dto)
        {
            var user = await _db.USER.FindAsync(id);
            if (user == null) return false;

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.UserName = dto.UserName;
            user.Email = dto.Email;
            user.Password = dto.Password; // Hash before save
            user.DateModification = DateTime.UtcNow;

            _db.USER.Update(user);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}