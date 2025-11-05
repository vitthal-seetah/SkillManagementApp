using SkillManager.Application.DTOs.Category;
using SkillManager.Application.DTOs.SubCategory;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Mappers;

public static class CategoryMappers
{
    public static CategoryDto ToCategoryDto(this Category category)
    {
        if (category == null)
            return new CategoryDto();

        return new CategoryDto
        {
            CategoryId = category.CategoryId,
            Name = category.Name,
            CategoryTypeId = category.CategoryTypeId,
            CategoryTypeName = category.CategoryType?.Name ?? string.Empty,
            SkillCount = category.Skills?.Count ?? 0,
            SubCategoryCount = category.SubCategories?.Count ?? 0,
            SubCategories =
                category.SubCategories?.Select(sc => sc.ToSubCategoryDto()).ToList()
                ?? new List<SubCategoryDto>(),
        };
    }

    public static CategoryTypeDto ToCategoryTypeDto(this CategoryType categoryType)
    {
        if (categoryType == null)
            return new CategoryTypeDto();

        return new CategoryTypeDto
        {
            CategoryTypeId = categoryType.CategoryTypeId,
            Name = categoryType.Name,
            CategoryCount = categoryType.Categories?.Count ?? 0,
        };
    }

    public static Category ToCategory(this CreateCategoryDto createDto)
    {
        return new Category
        {
            Name = createDto.Name.Trim(),
            CategoryTypeId = createDto.CategoryTypeId,
        };
    }

    public static CategoryType ToCategoryType(this CreateCategoryTypeDto createDto)
    {
        return new CategoryType { Name = createDto.Name.Trim() };
    }
}
