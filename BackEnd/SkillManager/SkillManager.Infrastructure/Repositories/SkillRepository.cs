namespace SkillManager.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using global::SkillManager.Application.Interfaces.Repositories;
    using global::SkillManager.Domain.Entities;
    using global::SkillManager.Infrastructure.Identity.AppDbContext;
    using Microsoft.EntityFrameworkCore;

    namespace SkillManager.Infrastructure.Repositories
    {
        public class SkillRepository : ISkillRepository
        {
            private readonly ApplicationDbContext _context;

            public SkillRepository(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Skill?> GetByIdAsync(int skillId)
            {
                return await _context
                    .Skills.Include(s => s.Category)
                    .ThenInclude(c => c.CategoryType)
                    .Include(s => s.SubCategory)
                    .FirstOrDefaultAsync(s => s.SkillId == skillId);
            }

            public async Task<Skill?> GetByCodeAsync(string code)
            {
                return await _context
                    .Skills.Include(s => s.Category)
                    .Include(s => s.SubCategory)
                    .FirstOrDefaultAsync(s => s.Code == code);
            }

            public async Task<IEnumerable<Skill>> GetAllAsync()
            {
                return await _context
                    .Skills.Include(s => s.Category) // Include Category
                    .ThenInclude(c => c.CategoryType) // Then include CategoryType from Category
                    .Include(s => s.SubCategory) // Include SubCategory
                    .OrderBy(s => s.Code)
                    .ToListAsync();
            }

            public async Task<IEnumerable<Skill>> GetByCategoryAsync(Category category)
            {
                return await _context
                    .Skills.Include(s => s.Category)
                    .ThenInclude(c => c.CategoryType)
                    .Include(s => s.SubCategory)
                    .Where(s => s.CategoryId == category.CategoryId)
                    .OrderBy(s => s.Code)
                    .ToListAsync();
            }

            public async Task<IEnumerable<Skill>> GetBySubCategoryAsync(SubCategory subCategory)
            {
                return await _context
                    .Skills.Include(s => s.Category)
                    .ThenInclude(c => c.CategoryType) // CategoryType from Category
                    .Include(s => s.SubCategory)
                    .Where(s => s.SubCategoryId == subCategory.SubCategoryId)
                    .OrderBy(s => s.Code)
                    .ToListAsync();
            }

            public async Task<IEnumerable<Skill>> GetCriticalSkillsAsync()
            {
                return await _context
                    .Skills.Include(s => s.Category)
                    .ThenInclude(c => c.CategoryType)
                    .Include(s => s.SubCategory)
                    .Where(s => s.ProjectRequiresSkill == true)
                    .OrderBy(s => s.Code)
                    .ToListAsync();
                ;
            }

            public async Task<IEnumerable<Skill>> GetProjectRequiredSkillsAsync()
            {
                return await _context
                    .Skills.Include(s => s.Category)
                    .ThenInclude(c => c.CategoryType)
                    .Include(s => s.SubCategory)
                    .Where(s => s.ProjectRequiresSkill == true)
                    .OrderBy(s => s.Code)
                    .ToListAsync();
            }

            public async Task<bool> AddAsync(Skill skill)
            {
                try
                {
                    _context.Skills.Add(skill);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public async Task<bool> UpdateAsync(Skill skill)
            {
                try
                {
                    _context.Skills.Update(skill);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public async Task<bool> DeleteAsync(Skill skill)
            {
                try
                {
                    _context.Skills.Remove(skill);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public async Task<bool> ExistsAsync(int skillId)
            {
                return await _context.Skills.AnyAsync(s => s.SkillId == skillId);
            }
        }
    }
}
