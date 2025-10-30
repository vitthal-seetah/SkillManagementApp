using SkillManager.Domain.Entities;

namespace SkillManager.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int userId);
    Task<IEnumerable<User>> GetAllAsync();
    Task UpdateAsync(User user);
    Task<UserRole?> GetRoleByNameAsync(string roleName);
    Task<User?> GetByUtCodeAsync(string utCode);
    Task AddAsync(User user);
    Task<User?> GetByRefIdAsync(string refId);
    Task SaveChangesAsync();
}
