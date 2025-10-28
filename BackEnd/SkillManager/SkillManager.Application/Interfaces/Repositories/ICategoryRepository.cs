using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(int id);
        Task<SubCategory?> GetSubCategoryByIdAsync(int id);

        Task<IEnumerable<Category>> GetAllAsync();

        Task<bool> AddAsync(Category category);
        Task<bool> DeleteAsync(Category category);
        Task<bool> UpdateAsync(Category category);
        Task<IEnumerable<Category>> GetByCategoryTypeAsync(CategoryType categoryType);

        Task<bool> ExistsAsync(int categoryId);
    }
}
