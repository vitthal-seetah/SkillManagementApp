using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Application.DTOs.Skill;

namespace SkillManager.Application.Interfaces.Repositories
{
    public interface ISkillService
    {
        // Basic CRUD Operations
        Task<SkillDto> GetSkillByIdAsync(int skillId);
        Task<SkillDto> GetSkillByCodeAsync(string code);
        Task<IEnumerable<SkillDto>> GetAllSkillsAsync();
        Task<bool> CreateSkillAsync(CreateSkillDto createDto);
        Task<SkillDto> UpdateSkillAsync(int skillId, UpdateSkillDto updateDto);
        Task<bool> DeleteSkillAsync(int skillId);

        // Category-based Operations
        Task<IEnumerable<SkillDto>> GetSkillsByCategoryAsync(int categoryId);

        // SubCategory-based Operations
        Task<IEnumerable<SkillDto>> GetSkillsBySubCategoryAsync(int subCategoryId);

        // Specialized Queries
        Task<IEnumerable<SkillDto>> GetCriticalSkillsAsync();
        Task<IEnumerable<SkillDto>> GetProjectRequiredSkillsAsync();
        Task<IEnumerable<SkillDto>> SearchSkillsAsync(string searchTerm);
    }
}
