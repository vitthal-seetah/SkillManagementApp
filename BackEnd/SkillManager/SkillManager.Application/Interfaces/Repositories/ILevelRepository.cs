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
        Task<bool> AddAsync(Level level);

        Task<IEnumerable<Level>> GetAllAsync();
        Task<bool> UpdateAsync(Level level);

        Task<bool> DeleteAsync(Level level);

        Task<bool> ExistsAsync(int id);
    }
}
