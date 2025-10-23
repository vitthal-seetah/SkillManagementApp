using SkillManager.Domain.Entities;

namespace SkillManager.Infrastructure.Abstractions.Repository;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int userId);
    Task<IEnumerable<User>> GetAllAsync();
    Task UpdateAsync(User user);
}
