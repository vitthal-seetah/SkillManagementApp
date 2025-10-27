using SkillManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Application.Interfaces.Repositories
{
    public interface IUserSkillRepository
    {
        Task<UserSkill> GetUserSkillAsync(int userId, int skillId);
        Task<IEnumerable<UserSkill>> GetUserSkillsAsync(int userId);
        Task<IEnumerable<UserSkill>> GetSkillsUsersAsync(int skillId);
        Task<IEnumerable<UserSkill>> GetUsersBySkillLevelAsync(int skillId, int levelId);
        Task AddUserSkillAsync(UserSkill userSkill);
        Task UpdateUserSkillAsync(UserSkill userSkill);
        Task RemoveUserSkillAsync(int userId, int skillId);
        Task<bool> UserSkillExistsAsync(int userId, int skillId);
        Task<IEnumerable<UserSkill>> GetSkillGapAnalysisAsync(int userId);
    }
}
