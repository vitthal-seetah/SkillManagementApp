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

        // ✅ Category Type Methods
        Task<IEnumerable<CategoryType>> GetAllCategoryTypesAsync();
        Task<CategoryType?> GetCategoryTypeByIdAsync(int id);

        // ✅ NEW: Additional methods needed for service
        Task<Category?> GetByNameAsync(string name);
        Task<bool> ExistsAsync(string categoryName);
        Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryIdAsync(int categoryId);
        Task<bool> AddSubCategoryAsync(SubCategory subCategory);
        Task<bool> UpdateSubCategoryAsync(SubCategory subCategory);
        Task<bool> DeleteSubCategoryAsync(int subCategoryId);
        Task<bool> SubCategoryExistsAsync(string subCategoryName, int categoryId);
        Task<bool> DeleteCategoryTypeAsync(int categoryTypeId);
        Task<bool> UpdateCategoryTypeAsync(CategoryType categoryType);

        Task<bool> AddCategoryTypeAsync(CategoryType categoryType);

        Task<CategoryType?> GetCategoryTypeByNameAsync(string name);
    }
}
