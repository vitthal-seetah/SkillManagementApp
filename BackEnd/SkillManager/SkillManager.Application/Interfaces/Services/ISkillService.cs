using SkillManager.Application.DTOs.Skill;

namespace SkillManager.Application.Interfaces.Repositories
{
    public interface ISkillService
    {
        // Basic CRUD Operations
        Task<SkillDto> GetSkillByIdAsync(int skillId);
        Task<SkillDto> GetSkillByCodeAsync(string code);
        Task<IEnumerable<SkillDto>> GetAllSkillsAsync();
        Task<bool> CreateSkillAsync(CreateSkillDto createDto); // Validator handles CreateSkillDto validation
        Task<SkillDto> UpdateSkillAsync(int skillId, UpdateSkillDto updateDto); // Validator handles UpdateSkillDto validation
        Task<bool> DeleteSkillAsync(int skillId);

        // Category-based Operations
        Task<IEnumerable<SkillDto>> GetSkillsByCategoryAsync(int categoryId);

        // SubCategory-based Operations
        Task<IEnumerable<SkillDto>> GetSkillsBySubCategoryAsync(int subCategoryId);

        // Specialized Queries
        Task<IEnumerable<SkillDto>> GetCriticalSkillsAsync();
        Task<IEnumerable<SkillDto>> GetProjectRequiredSkillsAsync();
        Task<IEnumerable<SkillDto>> GetSkillsByRequiredLevelAsync(int level); // Added to match service method
        Task<IEnumerable<SkillDto>> SearchSkillsAsync(string searchTerm);
    }
}
