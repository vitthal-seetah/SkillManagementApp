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
        // Get all users (with roles included)
        // ------------------------------------------------------
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.AsNoTracking().Include(u => u.Role).ToListAsync();
        }

        // ------------------------------------------------------
        // Get single user by ID
        // ------------------------------------------------------
        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context
                .Users.Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        // ------------------------------------------------------
        // Update user
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

            await _context.SaveChangesAsync();
        }

        public async Task<UserRole?> GetRoleByNameAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return null;

            return await _context.UserRoles.FirstOrDefaultAsync(r =>
                r.Name.ToLower() == roleName.ToLower()
            );
        }

        // ------------------------------------------------------
        // Optional: get user by domain + Eid (for Windows Auth)
        // ------------------------------------------------------
        public async Task<User?> GetByDomainAndEidAsync(string domain, string eid)
        {
            return await _context
                .Users.AsNoTracking()
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Domain == domain && u.Eid == eid);
        }
    }
}
