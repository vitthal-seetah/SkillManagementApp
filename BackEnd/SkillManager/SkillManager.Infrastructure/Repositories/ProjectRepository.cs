using Microsoft.EntityFrameworkCore;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.Identity.AppDbContext;

namespace SkillManager.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ------------------------------------------------------
        // Get all projects
        // ------------------------------------------------------
        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context
                .Projects.AsNoTracking()
                .Include(p => p.Users) // include users if needed
                .Include(p => p.ProjectTeams) // include teams if needed
                .ToListAsync();
        }

        // ------------------------------------------------------
        // Get project by ID
        // ------------------------------------------------------
        public async Task<Project?> GetByIdAsync(int projectId)
        {
            return await _context
                .Projects.Include(p => p.Users)
                .Include(p => p.ProjectTeams)
                .FirstOrDefaultAsync(p => p.ProjectId == projectId);
        }

        // ------------------------------------------------------
        // Add a new project
        // ------------------------------------------------------
        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
        }

        // ------------------------------------------------------
        // Update existing project
        // ------------------------------------------------------
        public async Task UpdateAsync(Project project)
        {
            var existingProject = await _context.Projects.FirstOrDefaultAsync(p =>
                p.ProjectId == project.ProjectId
            );

            if (existingProject == null)
                return;

            existingProject.ProjectName = project.ProjectName;
            existingProject.ProjectDescription = project.ProjectDescription;

            await _context.SaveChangesAsync();
        }

        // ------------------------------------------------------
        // Delete project
        // ------------------------------------------------------
        public async Task DeleteAsync(Project project)
        {
            _context.Projects.Remove(project);
        }

        // ------------------------------------------------------
        // Save changes
        // ------------------------------------------------------
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
