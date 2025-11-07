using SkillManager.Domain.Entities;

namespace SkillManager.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(int projectId);
        Task AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Project project);
        Task SaveChangesAsync();
    }
}
