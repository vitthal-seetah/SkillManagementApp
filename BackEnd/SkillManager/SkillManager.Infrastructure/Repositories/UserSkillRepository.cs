using Microsoft.EntityFrameworkCore;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.Identity.AppDbContext;

namespace SkillManager.Infrastructure.Persistence.Repositories;

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
            .Where(us => us.UserId == 3)
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
            .ToListAsync();
    }

    // -------------------------
    // Get a single UserSkill by composite key
    // -------------------------
    public async Task<UserSkill?> GetByCompositeKeyAsync(int userId, int skillId, int levelId)
    {
        return await _context
            .UserSkills.Include(us => us.Skill)
            .ThenInclude(s => s.Category)
            .ThenInclude(c => c.CategoryType)
            .Include(us => us.Level)
            .FirstOrDefaultAsync(us =>
                us.UserId == 4 && us.SkillId == skillId && us.LevelId == levelId
            );
    }

    // -------------------------
    // Add a new UserSkill
    // -------------------------
    public async Task AddAsync(UserSkill userSkill)
    {
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
    // ✅ Fixed: Delete a UserSkill (by composite key)
    // -------------------------
    public async Task DeleteAsync(int userId)
    {
        var userSkill = await _context.UserSkills.FirstOrDefaultAsync(us => us.UserId == 4);

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
            .Where(us => us.Skill.Label.Contains(skillName))
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
