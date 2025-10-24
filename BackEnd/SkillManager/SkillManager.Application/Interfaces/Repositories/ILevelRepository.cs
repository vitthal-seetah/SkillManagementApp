using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Interfaces.Repositories
{
    public interface ILevelRepository
    {
        Task<Level> GetByIdAsync(int id);

        Task<IEnumerable<Level>> GetAllAsync();
        Task<bool> UpdateAsync(Level level);

        Task DeleteAsync(Level level);

        Task<bool> ExistsAsync(int id);
    }
}
