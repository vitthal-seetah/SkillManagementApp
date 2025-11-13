using Microsoft.EntityFrameworkCore;
using SkillManager.Application.DTOs.Category;
using SkillManager.Application.DTOs.Skill;
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

        public async Task<IEnumerable<UserSkill>?> GetSkillsByCategory(Category category, User user)
        {
            return await _context
                .UserSkills.Include(us => us.Skill)
                    .ThenInclude(s => s.Category)
                .Include(us => us.Level)
                .Where(us => us.UserId == user.UserId && us.Skill.CategoryId == category.CategoryId)
                .OrderBy(us => us.Skill.Code)
                .ToListAsync();
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

        public async Task<IEnumerable<UserSkill>> GetAllByTeamAsync(User user)
        {
            return await _context
                .UserSkills.Include(us => us.Skill)
                    .ThenInclude(s => s.Category)
                        .ThenInclude(c => c.CategoryType)
                .Include(us => us.Level)
                .Include(us => us.User)
                    .ThenInclude(us => us.Team)
                .Where(us => us.User.TeamId == user.TeamId && us.User.ProjectId == user.ProjectId)
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

        public async Task<IEnumerable<UserSkill>> GetAllUserSkillsLevels()
        {
            return await _context
                .UserSkills.Include(us => us.Skill)
                    .ThenInclude(s => s.Category)
                .Include(us => us.Level)
                .Include(us => us.User)
                .ToListAsync();
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
                .UserSkills.Include(us => us.User)
                .Include(us => us.Skill)
                    .ThenInclude(s => s.Category)
                        .ThenInclude(c => c.CategoryType)
                .Include(us => us.Level)
                .Where(us =>
                    EF.Functions.Like(us.Skill.Code, $"%{skillName}%")
                    || EF.Functions.Like(us.Skill.Label, $"%{skillName}%")
                )
                .ToListAsync();
        }

        public async Task<List<CategoryGapDto>> GetSkillGapsByCategoryAsync(int userId)
        {
            var skillGaps = await GetSkillGapsAsync(userId);

            return skillGaps
                .GroupBy(sg => new { sg.CategoryId, sg.CategoryName })
                .Select(g => new CategoryGapDto
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryName = g.Key.CategoryName,
                    Skills = g.ToList(),
                    AverageGap = (int)g.Average(x => x.GapSize),
                })
                .ToList();
        }

        public async Task<List<SkillGapDto>> GetSkillGapsAsync(int userId)
        {
            var userSkillsWithGaps = await _context
                .UserSkills.Where(us => us.UserId == userId)
                .Include(us => us.Skill)
                    .ThenInclude(s => s.Category)
                .Include(us => us.Skill)
                .Include(us => us.Level) // User's current level
                .Select(us => new SkillGapDto
                {
                    SkillId = us.SkillId,
                    SkillCode = us.Skill.Code,
                    SkillName = us.Skill.Label,
                    CategoryId = us.Skill.CategoryId,
                    CategoryName = us.Skill.Category.Name,
                    UserLevel = us.Level.Points,
                    UserLevelName = us.Level.Name,
                    RequiredLevel = us.Skill.RequiredLevel,
                    RequiredLevelCode = us.Skill.Code,
                    RequiredLevelLabel = us.Skill.Label,
                    ProjectRequiresSkill = us.Skill.ProjectRequiresSkill,
                })
                .ToListAsync();

            return userSkillsWithGaps;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
