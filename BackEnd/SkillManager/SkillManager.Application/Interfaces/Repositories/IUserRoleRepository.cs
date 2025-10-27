using SkillManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Application.Interfaces.Repositories
{
    public interface IUserRoleRepository
    {

        Task<bool> IsAdminRoleAsync(User user);
        Task<bool> IsTechLeadRoleAsync(User user);
        Task<bool> IsManagerRoleAsync(User user);
        Task<bool> IsEmployeeRoleAsync(User user);
        Task<bool> ExistsAsync(int roleId);

    }
}
