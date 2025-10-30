using SkillManager.Application.DTOs.Category;
using SkillManager.Application.DTOs.Skill;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Interfaces.Repositories
{
    public interface IUserSkillRepository
    {
        // Get skills for a specific user
        Task<IEnumerable<UserSkill>> GetUserSkillsAsync(int userId);

        // Get all user skills
        Task<IEnumerable<UserSkill>> GetAllAsync();

        // Get a single UserSkill by userId + skillId (composite key)
        Task<UserSkill?> GetByCompositeKeyAsync(int userId, int skillId);

        // Add a new UserSkill
        Task AddAsync(UserSkill userSkill);

        Task<IEnumerable<UserSkill>?> GetSkillsByCategory(Category category, User user);

        // Update an existing UserSkill
        Task UpdateAsync(UserSkill userSkill);
        Task<IEnumerable<UserSkill>> GetAllUserSkillsLevels();

        // Delete a UserSkill by userId + skillId
        Task DeleteAsync(int userId, int skillId);

        // Filter skills by skill name
        Task<IEnumerable<UserSkill>> FilterBySkillAsync(string skillName);

        // Save changes to the database
        Task SaveChangesAsync();
        Task<List<CategoryGapDto>> GetSkillGapsByCategoryAsync(int userId);
        Task<List<SkillGapDto>> GetSkillGapsAsync(int userId);
    }
}
