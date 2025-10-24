using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Domain.Entities;
using AppEntity = SkillManager.Domain.Entities.Application;

namespace SkillManager.Application.Interfaces.Repositories;

public interface IApplicationRepository
{
    Task<AppEntity> GetByIdAsync(int applicationId);
    Task<IEnumerable<AppEntity>> GetAllAsync();
    Task<IEnumerable<AppEntity>> GetBySuiteAsync(ApplicationSuite suite);
    Task<IEnumerable<AppEntity>> GetByCategoryAsync(Category category);
    Task AddAsync(AppEntity application);
    Task UpdateAsync(AppEntity application);
    Task DeleteAsync(AppEntity application);

    // Application Skills management
    Task AddSkillToApplicationAsync(Skill skill);
    Task RemoveSkillFromApplicationAsync(AppEntity application, Skill skill);
    Task<IEnumerable<Skill>> GetApplicationSkillsAsync(AppEntity application);
    Task<IEnumerable<AppEntity>> GetApplicationsBySkillAsync(Skill skill);
}
