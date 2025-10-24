using SkillManager.Domain.Entities;

namespace SkillManager.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int userId);
    Task<IEnumerable<User>> GetAllAsync();
    Task UpdateAsync(User user);
}
