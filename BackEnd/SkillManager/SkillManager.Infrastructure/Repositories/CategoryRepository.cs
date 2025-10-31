using Microsoft.EntityFrameworkCore;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.Identity.AppDbContext;

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
                .ThenInclude(sc => sc.Skills)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<SubCategory?> GetSubCategoryByIdAsync(int id)
        {
            return await _context
                .SubCategories.Include(sc => sc.Skills) // Add this line for SkillCount
                .Include(sc => sc.Category) // Add this line for CategoryName
                .FirstOrDefaultAsync(sc => sc.SubCategoryId == id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context
                .Categories.Include(c => c.CategoryType)
                .Include(c => c.Skills)
                .Include(c => c.SubCategories)
                .ThenInclude(sc => sc.Skills)
                .OrderBy(c => c.CategoryType.Name)
                .ThenBy(c => c.Name)
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
                .ThenInclude(sc => sc.Skills)
                .Where(c => c.CategoryType == categoryType)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.CategoryId == categoryId);
        }

        public async Task<IEnumerable<CategoryType>> GetAllCategoryTypesAsync()
        {
            return await _context.CategoryTypes.OrderBy(ct => ct.Name).ToListAsync();
        }

        public async Task<CategoryType?> GetCategoryTypeByIdAsync(int id)
        {
            return await _context.CategoryTypes.FirstOrDefaultAsync(ct => ct.CategoryTypeId == id);
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _context
                .Categories.Include(c => c.SubCategories)
                .Include(c => c.CategoryType)
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<bool> ExistsAsync(string categoryName)
        {
            return await _context.Categories.AnyAsync(c => c.Name == categoryName);
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryIdAsync(
            int categoryId
        )
        {
            return await _context
                .SubCategories.Include(sc => sc.Skills) // This is crucial for SkillCount to work
                .Include(sc => sc.Category)
                .Where(sc => sc.CategoryId == categoryId)
                .OrderBy(sc => sc.Name)
                .ToListAsync();
        }

        public async Task<bool> AddSubCategoryAsync(SubCategory subCategory)
        {
            try
            {
                await _context.SubCategories.AddAsync(subCategory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateSubCategoryAsync(SubCategory subCategory)
        {
            try
            {
                _context.SubCategories.Update(subCategory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteSubCategoryAsync(int subCategoryId)
        {
            try
            {
                var subCategory = await _context.SubCategories.FindAsync(subCategoryId);
                if (subCategory == null)
                    return false;

                _context.SubCategories.Remove(subCategory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SubCategoryExistsAsync(string subCategoryName, int categoryId)
        {
            return await _context.SubCategories.AnyAsync(sc =>
                sc.Name == subCategoryName && sc.CategoryId == categoryId
            );
        }
    }
}
