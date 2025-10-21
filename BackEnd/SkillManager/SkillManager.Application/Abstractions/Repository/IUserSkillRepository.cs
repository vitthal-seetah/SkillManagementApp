using SkillManager.Domain.Entities;

public interface IUserSkillRepository
{
    Task<IEnumerable<UserSkill>> GetUserSkillsAsync(string userId);
    Task<IEnumerable<UserSkill>> GetAllAsync();
    Task<UserSkill?> GetByCompositeKeyAsync(string userId, int skillId, int levelId);
    Task AddAsync(UserSkill userSkill);
    Task UpdateAsync(UserSkill userSkill);
    Task DeleteAsync(string userId);
    Task<IEnumerable<UserSkill>> FilterBySkillAsync(string skillName);
    Task SaveChangesAsync();
}
