using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Interfaces.Repositories
{
    public interface ISkillRepository
    {
        Task<Skill?> GetByIdAsync(int skillId);
        Task<Skill?> GetByCodeAsync(string code);
        Task<IEnumerable<Skill>> GetAllAsync();
        Task<IEnumerable<Skill>> GetByCategoryAsync(Category category);
        Task<IEnumerable<Skill>> GetBySubCategoryAsync(SubCategory subCatgory);
        Task<IEnumerable<Skill>> GetCriticalSkillsAsync();
        Task<IEnumerable<Skill>> GetProjectRequiredSkillsAsync();
        Task<bool> AddAsync(Skill skill);
        Task<bool> UpdateAsync(Skill skill);
        Task<bool> DeleteAsync(Skill skill);
        Task<bool> ExistsAsync(int skillId);
    }
}
