using Microsoft.EntityFrameworkCore;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.Identity.AppDbContext;

namespace SkillManager.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ------------------------------------------------------
        // Get all users (with roles, projects, teams included)
        // ------------------------------------------------------
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context
                .Users.AsNoTracking()
                .Include(u => u.Role)
                .Include(u => u.Project)
                .Include(u => u.Team)
                .ToListAsync();
        }

        // ------------------------------------------------------
        // Get single user by ID
        // ------------------------------------------------------
        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context
                .Users.Include(u => u.Role)
                .Include(u => u.Project)
                .Include(u => u.Team)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        // ------------------------------------------------------
        // Update existing user
        // ------------------------------------------------------
        public async Task UpdateAsync(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u =>
                u.UserId == user.UserId
            );

            if (existingUser == null)
                return;

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.UtCode = user.UtCode;
            existingUser.RefId = user.RefId;
            existingUser.RoleId = user.RoleId;
            existingUser.Status = user.Status;
            existingUser.DeliveryType = user.DeliveryType;
            existingUser.ProjectId = user.ProjectId;
            existingUser.TeamId = user.TeamId;

            if (!string.IsNullOrWhiteSpace(user.Domain))
                existingUser.Domain = user.Domain;

            if (!string.IsNullOrWhiteSpace(user.Eid))
                existingUser.Eid = user.Eid;

            await _context.SaveChangesAsync();
        }

        // ------------------------------------------------------
        // Get Role by Name
        // ------------------------------------------------------
        public async Task<UserRole?> GetRoleByNameAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return null;

            return await _context
                .UserRoles.AsNoTracking()
                .FirstOrDefaultAsync(r => r.Name.ToLower() == roleName.ToLower());
        }

        // ------------------------------------------------------
        // Get User by UT Code (for preventing duplicates)
        // ------------------------------------------------------
        public async Task<User?> GetByUtCodeAsync(string utCode)
        {
            if (string.IsNullOrWhiteSpace(utCode))
                return null;

            return await _context
                .Users.AsNoTracking()
                .Include(u => u.Role)
                .Include(u => u.Project)
                .Include(u => u.Team)
                .FirstOrDefaultAsync(u => u.UtCode == utCode);
        }

        // ------------------------------------------------------
        // Get User by RefId
        // ------------------------------------------------------
        public async Task<User?> GetByRefIdAsync(string refId)
        {
            if (string.IsNullOrWhiteSpace(refId))
                return null;

            return await _context
                .Users.AsNoTracking()
                .Include(u => u.Role)
                .Include(u => u.Project)
                .Include(u => u.Team)
                .FirstOrDefaultAsync(u => u.RefId == refId);
        }

        // ------------------------------------------------------
        // Add New User
        // ------------------------------------------------------
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        // ------------------------------------------------------
        // Save Changes
        // ------------------------------------------------------
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // ------------------------------------------------------
        //Get user by Domain + Eid
        // ------------------------------------------------------
        public async Task<User?> GetByDomainAndEidAsync(string? domain, string eid)
        {
            if (string.IsNullOrWhiteSpace(eid))
                return null;

            var query = _context
                .Users.Include(u => u.Role)
                .Include(u => u.Project)
                .Include(u => u.Team)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(domain))
                query = query.Where(u => u.Domain == domain);

            return await query.FirstOrDefaultAsync(u => u.Eid == eid);
        }

        public async Task<IEnumerable<User?>> GetByProjectIdAsync(int? projectId)
        {
            if (projectId == null || projectId == 0)
                return Enumerable.Empty<User>();

            return await _context
                .Users.Include(u => u.Role)
                .Include(u => u.Project)
                .Include(u => u.Team)
                .Where(u => u.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetByProjectAndTeamAsync(User user)
        {
            if (user.ProjectId == null || user.ProjectId == 0)
                return Enumerable.Empty<User>();

            return await _context
                .Users.Include(u => u.Role)
                .Include(u => u.Project)
                .Include(u => u.Team)
                .Where(u => u.ProjectId == user.ProjectId && u.TeamId == user.TeamId)
                .ToListAsync();
        }
    }
}
