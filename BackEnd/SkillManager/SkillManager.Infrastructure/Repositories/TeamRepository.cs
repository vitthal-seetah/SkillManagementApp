using Microsoft.EntityFrameworkCore;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.Identity.AppDbContext;

namespace SkillManager.Infrastructure.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _context;

        public TeamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Team?> GetByIdAsync(int teamId)
        {
            return await _context.Teams.FirstOrDefaultAsync(t => t.TeamId == teamId);
        }

        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            return await _context.Teams.OrderBy(t => t.TeamName).ToListAsync();
        }

        public async Task<IEnumerable<Team>> GetTeamsByLeadAsync(User teamLead)
        {
            return await _context
                .Teams.Where(t => t.TeamLeadId == teamLead.UserId)
                .OrderBy(t => t.TeamName)
                .ToListAsync();
        }

        public async Task<Team> AddAsync(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            return team;
        }

        public async Task<Team> UpdateAsync(Team team)
        {
            _context.Teams.Update(team);
            await _context.SaveChangesAsync();
            return team;
        }

        public async Task<bool> DeleteAsync(Team team)
        {
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int teamId)
        {
            return await _context.Teams.AnyAsync(t => t.TeamId == teamId);
        }

        public async Task<bool> IsTeamLeadAsync(User user)
        {
            return await _context.Teams.AnyAsync(t => t.TeamLeadId == user.UserId);
        }

        public async Task<IEnumerable<User>> GetTeamMembersAsync(Team team)
        {
            return await _context
                .Users.Where(u => u.TeamId == team.TeamId)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToListAsync();
        }

        public async Task<bool> AddUserToTeamAsync(Team team, User user)
        {
            user.TeamId = team.TeamId;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserFromTeamAsync(Team team, User user)
        {
            if (user.TeamId == team.TeamId)
            {
                user.TeamId = null;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Team?> GetTeamWithMembersAsync(int teamId)
        {
            return await _context
                .Teams.Include(t => t.Members)
                .Include(t => t.TeamLead)
                .FirstOrDefaultAsync(t => t.TeamId == teamId);
        }

        public async Task<Team?> GetTeamWithProjectsAsync(int teamId)
        {
            return await _context
                .Teams.Include(t => t.ProjectTeams)
                .ThenInclude(pt => pt.Project)
                .FirstOrDefaultAsync(t => t.TeamId == teamId);
        }

        public async Task<IEnumerable<Team>> GetAllWithProjectsAsync()
        {
            return await _context
                .Teams.Include(t => t.ProjectTeams)
                .ThenInclude(pt => pt.Project)
                .ToListAsync();
        }

        public async Task<IEnumerable<Team>> GetTeamsByProjectIdAsync(int? projectId)
        {
            return await _context
                .Teams.Include(t => t.ProjectTeams)
                .Where(t => t.ProjectTeams.Any(pt => pt.ProjectId == projectId))
                .ToListAsync();
        }

        // Add team to project association
        public async Task AddTeamToProjectAsync(int teamId, int projectId)
        {
            // Check if the association already exists
            var existingAssociation = await _context.ProjectTeams.FirstOrDefaultAsync(pt =>
                pt.TeamId == teamId && pt.ProjectId == projectId
            );

            if (existingAssociation == null)
            {
                var projectTeam = new ProjectTeam { TeamId = teamId, ProjectId = projectId };

                _context.ProjectTeams.Add(projectTeam);
                await _context.SaveChangesAsync();
            }
        }

        //  Remove team from project association
        public async Task RemoveTeamFromProjectAsync(int teamId, int projectId)
        {
            var projectTeam = await _context.ProjectTeams.FirstOrDefaultAsync(pt =>
                pt.TeamId == teamId && pt.ProjectId == projectId
            );

            if (projectTeam != null)
            {
                _context.ProjectTeams.Remove(projectTeam);
                await _context.SaveChangesAsync();
            }
        }

        // Update team's project association
        public async Task UpdateTeamProjectAsync(int teamId, int newProjectId)
        {
            // Remove existing project associations for this team
            var existingAssociations = await _context
                .ProjectTeams.Where(pt => pt.TeamId == teamId)
                .ToListAsync();

            _context.ProjectTeams.RemoveRange(existingAssociations);

            // Add new association
            var newProjectTeam = new ProjectTeam { TeamId = teamId, ProjectId = newProjectId };

            _context.ProjectTeams.Add(newProjectTeam);
            await _context.SaveChangesAsync();
        }

        //  Get project ID for a team
        public async Task<int?> GetProjectIdForTeamAsync(int teamId)
        {
            var projectTeam = await _context.ProjectTeams.FirstOrDefaultAsync(pt =>
                pt.TeamId == teamId
            );

            return projectTeam?.ProjectId;
        }

        //Check if team is associated with project
        public async Task<bool> IsTeamInProjectAsync(int teamId, int projectId)
        {
            return await _context.ProjectTeams.AnyAsync(pt =>
                pt.TeamId == teamId && pt.ProjectId == projectId
            );
        }

        public async Task AddProjectTeamAsync(ProjectTeam projectTeam)
        {
            _context.ProjectTeams.Add(projectTeam);
            await _context.SaveChangesAsync();
        }
    }
}
