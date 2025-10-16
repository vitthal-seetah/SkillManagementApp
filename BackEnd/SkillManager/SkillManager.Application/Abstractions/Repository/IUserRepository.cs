using SkillManager.Application.Abstractions.Identity;

namespace SkillManager.Application.Abstractions.Repository;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(string userId);
    Task<IEnumerable<User>> GetAllAsync();
    Task UpdateAsync(User user);
}
