using SkillManager.Application.DTOs.Category;
using SkillManager.Application.DTOs.SubCategory;

public interface ICategoryService
{
    // Category methods
    Task<CategoryDto> GetCategoryByIdAsync(int categoryId);
    Task<CategoryDto> GetCategoryByNameAsync(string name);
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    Task<IEnumerable<CategoryDto>> GetCategoriesByTypeAsync(int categoryTypeId);
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto);
    Task<CategoryDto> UpdateCategoryAsync(int categoryId, UpdateCategoryDto updateDto);
    Task<bool> DeleteCategoryAsync(int categoryId);

    // Category Type methods
    Task<CategoryTypeDto> GetCategoryTypeByIdAsync(int categoryTypeId);
    Task<CategoryTypeDto> GetCategoryTypeByNameAsync(string name);
    Task<IEnumerable<CategoryTypeDto>> GetAllCategoryTypesAsync();
    Task<CategoryTypeDto> CreateCategoryTypeAsync(CreateCategoryTypeDto createDto);
    Task<CategoryTypeDto> UpdateCategoryTypeAsync(
        int categoryTypeId,
        UpdateCategoryTypeDto updateDto
    );
    Task<bool> DeleteCategoryTypeAsync(int categoryTypeId);

    // SubCategory methods
    Task<SubCategoryDto> GetSubCategoryByIdAsync(int subCategoryId);
    Task<IEnumerable<SubCategoryDto>> GetSubCategoriesByCategoryAsync(int categoryId);
    Task<SubCategoryDto> CreateSubCategoryAsync(CreateSubCategoryDto createDto);
    Task<SubCategoryDto> UpdateSubCategoryAsync(int subCategoryId, UpdateSubCategoryDto updateDto);
    Task<bool> DeleteSubCategoryAsync(int subCategoryId);

    // Validation methods
    Task<bool> CategoryExistsAsync(int categoryId);
    Task<bool> CategoryExistsAsync(string categoryName);
    Task<bool> CategoryTypeExistsAsync(int categoryTypeId);
    Task<bool> CategoryTypeExistsAsync(string categoryTypeName);
    Task<bool> SubCategoryExistsAsync(int subCategoryId);
    Task<bool> SubCategoryExistsAsync(string subCategoryName, int categoryId);
}
