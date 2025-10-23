using SkillManager.Domain.Entities;

public interface IUserSkillRepository
{
    Task<IEnumerable<UserSkill>> GetUserSkillsAsync(int userId);
    Task<IEnumerable<UserSkill>> GetAllAsync();
    Task<UserSkill?> GetByCompositeKeyAsync(int userId, int skillId, int levelId);
    Task AddAsync(UserSkill userSkill);
    Task UpdateAsync(UserSkill userSkill);
    Task DeleteAsync(int userId);
    Task<IEnumerable<UserSkill>> FilterBySkillAsync(string skillName);
    Task SaveChangesAsync();
}
