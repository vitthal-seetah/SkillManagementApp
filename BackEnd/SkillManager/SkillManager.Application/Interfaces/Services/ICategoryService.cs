using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Application.DTOs.Category;
using SkillManager.Application.DTOs.SubCategory;
using SkillManager.Application.Models;

namespace SkillManager.Application.Interfaces.Services;

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
    Task<IEnumerable<CategoryTypeDto>> GetAllCategoryTypesAsync();

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
    Task<bool> SubCategoryExistsAsync(int subCategoryId);
}
