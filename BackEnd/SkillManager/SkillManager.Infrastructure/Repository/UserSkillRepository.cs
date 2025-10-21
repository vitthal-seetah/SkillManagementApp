using Microsoft.EntityFrameworkCore;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.Abstractions.Repository;
using SkillManager.Infrastructure.Identity.DbContext;

namespace SkillManager.Infrastructure.Persistence.Repositories;

public class UserSkillRepository : IUserSkillRepository
{
    private readonly ApplicationIdentityDbContext _context;

    public UserSkillRepository(ApplicationIdentityDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserSkill>> GetUserSkillsAsync(string userId)
    {
        return await _context
            .UserSkills.Include(us => us.Skill)
            .ThenInclude(s => s.SkillSection)
            .ThenInclude(ss => ss.Category)
            .Where(us => us.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserSkill>> GetAllAsync()
    {
        return await _context
            .UserSkills.Include(us => us.Skill)
            .Include(us => us.Skill.SkillSection)
            .Include(us => us.Skill.SkillSection.Category)
            .ToListAsync();
    }

    public async Task<UserSkill?> GetByIdAsync(int id)
    {
        return await _context
            .UserSkills.Include(us => us.Skill)
            .FirstOrDefaultAsync(us => us.Id == id);
    }

    public async Task AddAsync(UserSkill userSkill)
    {
        await _context.UserSkills.AddAsync(userSkill);
    }

    public async Task UpdateAsync(UserSkill userSkill)
    {
        _context.UserSkills.Update(userSkill);
    }

    public async Task DeleteAsync(int id)
    {
        var userSkill = await _context.UserSkills.FindAsync(id);
        if (userSkill != null)
            _context.UserSkills.Remove(userSkill);
    }

    public async Task<IEnumerable<UserSkill>> FilterBySkillAsync(string skillName)
    {
        return await _context
            .UserSkills.Include(us => us.Skill)
            .Where(us => us.Skill.Name.Contains(skillName))
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
