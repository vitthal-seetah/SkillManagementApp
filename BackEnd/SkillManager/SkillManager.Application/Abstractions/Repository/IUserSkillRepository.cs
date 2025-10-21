using SkillManager.Domain.Entities;

namespace SkillManager.Infrastructure.Abstractions.Repository;

public interface IUserSkillRepository
{
    Task<IEnumerable<UserSkill>> GetUserSkillsAsync(string userId);
    Task<UserSkill?> GetByIdAsync(int id);
    Task AddAsync(UserSkill userSkill);
    Task UpdateAsync(UserSkill userSkill);
    Task DeleteAsync(int id);
    Task<IEnumerable<UserSkill>> GetAllAsync();
    Task<IEnumerable<UserSkill>> FilterBySkillAsync(string skillName);
    Task SaveChangesAsync();
}
