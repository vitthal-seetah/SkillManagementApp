using SkillManager.Infrastructure.Abstractions.Identity;

namespace SkillManager.Infrastructure.Abstractions.Repository;

public interface IUserRepository
{
    Task<ApplicationUser?> GetByIdAsync(string userId);
    Task<IEnumerable<ApplicationUser>> GetAllAsync();
    Task UpdateAsync(ApplicationUser user);
}
