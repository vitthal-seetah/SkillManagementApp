using SkillManager.Application.DTOs.SubCategory;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Mappers;

public static class SubCategoryMapper
{
    public static SubCategory ToSubCategory(this CreateSubCategoryDto createDto)
    {
        return new SubCategory { Name = createDto.Name.Trim(), CategoryId = createDto.CategoryId };
    }

    public static SubCategoryDto ToSubCategoryDto(this SubCategory subCategory)
    {
        return new SubCategoryDto
        {
            SubCategoryId = subCategory.SubCategoryId,
            Name = subCategory.Name,
            CategoryId = subCategory.CategoryId,
            CategoryName = subCategory.Category?.Name ?? string.Empty,
            SkillCount = subCategory.Skills?.Count ?? 0,
        };
    }
}
