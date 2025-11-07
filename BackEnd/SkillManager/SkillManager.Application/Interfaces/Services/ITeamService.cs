using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Application.DTOs.Team;
using SkillManager.Application.DTOs.User;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Interfaces.Services
{
    public interface ITeamService
    {
        // Basic CRUD operations
        Task<Team?> GetTeamByIdAsync(int teamId);
        Task<IEnumerable<Team>> GetAllTeamsAsync();
        Task<IEnumerable<Team>> GetAllTeamsWithProjectsAsync();
        Task<Team> CreateTeamAsync(CreateTeamDto teamDto);
        Task<Team> UpdateTeamAsync(TeamDto teamDto);
        Task<bool> DeleteTeamAsync(int teamId);
        Task<bool> TeamExistsAsync(int teamId);

        // Team-specific operations
        Task<IEnumerable<Team>> GetTeamsByLeadAsync(UserDto teamLead);
        Task<bool> IsUserTeamLeadAsync(UserDto user);
        Task<IEnumerable<User>> GetTeamMembersAsync(int teamId);
        Task<bool> AddMemberToTeamAsync(int teamId, UserDto user);
        Task<bool> RemoveMemberFromTeamAsync(int teamId, UserDto user);
        Task<bool> SetTeamLeadAsync(int teamId, UserDto teamLead);

        // Advanced queries
        Task<Team?> GetTeamWithMembersAsync(int teamId);
        Task<Team?> GetTeamWithProjectsAsync(int teamId);

        // User-specific team operations
        Task<Team?> GetUserTeamAsync(User user);
        Task<bool> IsUserInTeamAsync(User user, Team team);
    }
}
