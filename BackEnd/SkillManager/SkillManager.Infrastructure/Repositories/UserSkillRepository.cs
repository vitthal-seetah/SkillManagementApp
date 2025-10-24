using Microsoft.EntityFrameworkCore;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.Identity.AppDbContext;

namespace SkillManager.Infrastructure.Persistence.Repositories
{
    public class UserSkillRepository : IUserSkillRepository
    {
        private readonly ApplicationDbContext _context;

        public UserSkillRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // -------------------------
        // Get skills for a specific user
        // -------------------------
        public async Task<IEnumerable<UserSkill>> GetUserSkillsAsync(int userId)
        {
            return await _context
                .UserSkills.Include(us => us.Skill)
                .ThenInclude(s => s.Category)
                .ThenInclude(c => c.CategoryType)
                .Include(us => us.Level)
                .Where(us => us.UserId == userId)
                .ToListAsync();
        }

        // -------------------------
        // Get all user skills
        // -------------------------
        public async Task<IEnumerable<UserSkill>> GetAllAsync()
        {
            return await _context
                .UserSkills.Include(us => us.Skill)
                .ThenInclude(s => s.Category)
                .ThenInclude(c => c.CategoryType)
                .Include(us => us.Level)
                .Include(us => us.User)
                .ToListAsync();
        }

        // -------------------------
        // Get a single UserSkill by composite key
        // -------------------------
        public async Task<UserSkill?> GetByCompositeKeyAsync(int userId, int skillId)
        {
            return await _context
                .UserSkills.Include(us => us.Skill)
                .ThenInclude(s => s.Category)
                .ThenInclude(c => c.CategoryType)
                .Include(us => us.Level)
                .FirstOrDefaultAsync(us => us.UserId == userId && us.SkillId == skillId);
        }

        // -------------------------
        // Add a new UserSkill
        // -------------------------
        public async Task AddAsync(UserSkill userSkill)
        {
            if (userSkill.User == null && userSkill.UserId == 0)
                throw new InvalidOperationException("UserId must be set for UserSkill.");

            await _context.UserSkills.AddAsync(userSkill);
        }

        // -------------------------
        // Update a UserSkill
        // -------------------------
        public async Task UpdateAsync(UserSkill userSkill)
        {
            _context.UserSkills.Update(userSkill);
        }

        // -------------------------
        // Delete a UserSkill (by user and skill)
        // -------------------------
        public async Task DeleteAsync(int userId, int skillId)
        {
            var userSkill = await _context.UserSkills.FirstOrDefaultAsync(us =>
                us.UserId == userId && us.SkillId == skillId
            );

            if (userSkill != null)
                _context.UserSkills.Remove(userSkill);
        }

        // -------------------------
        // Filter by skill name
        // -------------------------
        public async Task<IEnumerable<UserSkill>> FilterBySkillAsync(string skillName)
        {
            return await _context
                .UserSkills.Include(us => us.Skill)
                .ThenInclude(s => s.Category)
                .ThenInclude(c => c.CategoryType)
                .Include(us => us.Level)
                .Where(us => EF.Functions.Like(us.Skill.Label, $"%{skillName}%"))
                .ToListAsync();
        }

        // -------------------------
        // Save changes
        // -------------------------
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
