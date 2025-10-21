using SkillManager.Infrastructure.Abstractions.Identity;

namespace SkillManager.Infrastructure.Abstractions.Repository;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(string userId);
    Task<IEnumerable<User>> GetAllAsync();
    Task UpdateAsync(User user);
}
