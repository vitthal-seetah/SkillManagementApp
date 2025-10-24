using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Interfaces.Repositories;

public interface ISMERepository
{
    Task<UserSME> GetSMEAssignmentAsync(int userId, int skillId, int categoryTypeId);
    Task<IEnumerable<UserSME>> GetSMEsBySkillAsync(int skillId);
    Task<IEnumerable<UserSME>> GetSMEsByCategoryTypeAsync(int categoryTypeId);
    Task<IEnumerable<UserSME>> GetSMEAssignmentsByUserAsync(int userId);
    Task AssignSMEAsync(UserSME userSME);
    Task RemoveSMEAsync(int userId, int skillId, int categoryTypeId);
    Task<bool> IsSMEAsync(int userId, int skillId);
}
