using SkillManager.Application.DTOs;
using SkillManager.Application.DTOs.Team;
using SkillManager.Application.DTOs.User;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;

        public TeamService(ITeamRepository teamRepository, IUserRepository userRepository)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
        }

        // Basic CRUD operations
        public async Task<Team?> GetTeamByIdAsync(int teamId)
        {
            return await _teamRepository.GetByIdAsync(teamId);
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            return await _teamRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Team>> GetAllTeamsWithProjectsAsync()
        {
            return await _teamRepository.GetAllWithProjectsAsync();
        }

        public async Task<Team> CreateTeamAsync(CreateTeamDto teamDto)
        {
            // Convert DTO to entity
            var team = new Team
            {
                TeamName = teamDto.TeamName,
                TeamDescription = teamDto.TeamDescription,
                TeamLeadId = teamDto.TeamLeadId,
            };

            // Validate team lead exists if specified

            var teamLead = await _userRepository.GetByIdAsync(teamDto.TeamLeadId);
            if (teamLead == null)
                throw new ArgumentException("Team lead user not found");

            return await _teamRepository.AddAsync(team);
        }

        public async Task<Team> UpdateTeamAsync(TeamDto teamDto)
        {
            // Get existing team
            var existingTeam = await _teamRepository.GetByIdAsync(teamDto.TeamId);
            if (existingTeam == null)
                throw new ArgumentException("Team not found");

            // Update properties
            existingTeam.TeamName = teamDto.TeamName;
            existingTeam.TeamDescription = teamDto.TeamDescription;
            existingTeam.TeamLeadId = teamDto.TeamLeadId;

            // Validate team lead exists if specified

            var teamLead = await _userRepository.GetByIdAsync(teamDto.TeamLeadId);
            if (teamLead == null)
                throw new ArgumentException("Team lead user not found");

            return await _teamRepository.UpdateAsync(existingTeam);
        }

        public async Task<bool> DeleteTeamAsync(int teamId)
        {
            var team = await _teamRepository.GetByIdAsync(teamId);
            if (team == null)
                return false;

            // Check if team has members before deletion
            var members = await _teamRepository.GetTeamMembersAsync(team);
            if (members.Any())
                throw new InvalidOperationException(
                    "Cannot delete team that has members. Remove all members first."
                );

            return await _teamRepository.DeleteAsync(team);
        }

        public async Task<bool> TeamExistsAsync(int teamId)
        {
            return await _teamRepository.ExistsAsync(teamId);
        }

        // Team-specific operations
        public async Task<IEnumerable<Team>> GetTeamsByLeadAsync(UserDto teamLead)
        {
            var user = await _userRepository.GetByIdAsync(teamLead.UserId);
            if (user == null)
                return Enumerable.Empty<Team>();

            return await _teamRepository.GetTeamsByLeadAsync(user);
        }

        public async Task<bool> IsUserTeamLeadAsync(UserDto user)
        {
            var userEntity = await _userRepository.GetByIdAsync(user.UserId);
            if (userEntity == null)
                return false;

            return await _teamRepository.IsTeamLeadAsync(userEntity);
        }

        public async Task<IEnumerable<User>> GetTeamMembersAsync(int teamId)
        {
            var team = await _teamRepository.GetByIdAsync(teamId);
            if (team == null)
                return Enumerable.Empty<User>();

            return await _teamRepository.GetTeamMembersAsync(team);
        }

        public async Task<bool> AddMemberToTeamAsync(int teamId, UserDto user)
        {
            var team = await _teamRepository.GetByIdAsync(teamId);
            var userEntity = await _userRepository.GetByIdAsync(user.UserId);

            if (team == null || userEntity == null)
                return false;

            // Check if user is already in a team
            if (userEntity.TeamId.HasValue && userEntity.TeamId != teamId)
                throw new InvalidOperationException("User is already a member of another team");

            return await _teamRepository.AddUserToTeamAsync(team, userEntity);
        }

        public async Task<bool> RemoveMemberFromTeamAsync(int teamId, UserDto user)
        {
            var team = await _teamRepository.GetByIdAsync(teamId);
            var userEntity = await _userRepository.GetByIdAsync(user.UserId);

            if (team == null || userEntity == null)
                return false;

            // Check if user is the team lead
            if (team.TeamLeadId == user.UserId)
                throw new InvalidOperationException(
                    "Cannot remove team lead from team. Assign a new team lead first."
                );

            return await _teamRepository.RemoveUserFromTeamAsync(team, userEntity);
        }

        public async Task<bool> SetTeamLeadAsync(int teamId, UserDto teamLead)
        {
            var team = await _teamRepository.GetByIdAsync(teamId);
            var teamLeadEntity = await _userRepository.GetByIdAsync(teamLead.UserId);

            if (team == null || teamLeadEntity == null)
                return false;

            // Verify team lead is a member of the team
            var members = await _teamRepository.GetTeamMembersAsync(team);
            if (!members.Any(m => m.UserId == teamLead.UserId))
                throw new InvalidOperationException("Team lead must be a member of the team");

            team.TeamLeadId = teamLead.UserId;
            await _teamRepository.UpdateAsync(team);
            return true;
        }

        // Advanced queries
        public async Task<Team?> GetTeamWithMembersAsync(int teamId)
        {
            return await _teamRepository.GetTeamWithMembersAsync(teamId);
        }

        public async Task<Team?> GetTeamWithProjectsAsync(int teamId)
        {
            return await _teamRepository.GetTeamWithProjectsAsync(teamId);
        }

        // User-specific team operations
        public async Task<Team?> GetUserTeamAsync(User user)
        {
            if (!user.TeamId.HasValue)
                return null;

            return await _teamRepository.GetByIdAsync(user.TeamId.Value);
        }

        public async Task<bool> IsUserInTeamAsync(User user, Team team)
        {
            return user.TeamId == team.TeamId;
        }
    }
}
