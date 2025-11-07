using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Interfaces.Repositories
{
    public interface ITeamRepository
    {
        Task<Team?> GetByIdAsync(int teamId);
        Task<IEnumerable<Team>> GetAllAsync();
        Task<IEnumerable<Team>> GetAllWithProjectsAsync();
        Task<IEnumerable<Team>> GetTeamsByLeadAsync(User teamLead);
        Task<Team> AddAsync(Team team);
        Task<Team> UpdateAsync(Team team);
        Task<bool> DeleteAsync(Team team);
        Task<bool> ExistsAsync(int teamId);
        Task<bool> IsTeamLeadAsync(User user);
        Task<IEnumerable<User>> GetTeamMembersAsync(Team team);
        Task<bool> AddUserToTeamAsync(Team team, User user);
        Task<bool> RemoveUserFromTeamAsync(Team team, User user);
        Task<Team?> GetTeamWithMembersAsync(int teamId);
        Task<Team?> GetTeamWithProjectsAsync(int teamId);
    }
}
