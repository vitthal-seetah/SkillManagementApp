using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.Identity.AppDbContext;
using SkillManager.Infrastructure.Persistence;

namespace SkillManager.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context
                .Categories.Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<SubCategory?> GetSubCategoryByIdAsync(int id)
        {
            return await _context.SubCategories.FirstOrDefaultAsync(sc => sc.SubCategoryId == id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context
                .Categories.Include(c => c.SubCategories)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<bool> AddAsync(Category category)
        {
            try
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Category category)
        {
            try
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int categoryId)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category == null)
                    return false;

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            try
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Category>> GetByCategoryTypeAsync(CategoryType categoryType)
        {
            return await _context
                .Categories.Include(c => c.SubCategories)
                .Where(c => c.CategoryType == categoryType)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.CategoryId == categoryId);
        }

        public async Task<bool> ExistsAsync(string categoryName)
        {
            return await _context.Categories.AnyAsync(c => c.Name == categoryName);
        }

        // Additional helpful methods
        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _context
                .Categories.Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryIdAsync(
            int categoryId
        )
        {
            return await _context
                .SubCategories.Where(sc => sc.CategoryId == categoryId)
                .OrderBy(sc => sc.Name)
                .ToListAsync();
        }
    }
}
