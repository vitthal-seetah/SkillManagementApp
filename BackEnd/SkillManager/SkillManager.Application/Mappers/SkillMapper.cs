using SkillManager.Application.DTOs.Skill;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Mappers;

public static class SkillMapper
{
    public static Skill ToSkill(this CreateSkillDto createSkillDto)
    {
        return new Skill
        {
            Code = createSkillDto.Code,
            Label = createSkillDto.Label,
            CategoryId = createSkillDto.CategoryId,
            SubCategoryId = createSkillDto.SubCategoryId,
            CriticalityLevel = createSkillDto.CriticalityLevel,
            ProjectRequiresSkill = createSkillDto.ProjectRequiresSkill,
            RequiredLevel = createSkillDto.RequiredLevel,
            FirstLevelDescription = createSkillDto.FirstLevelDescription,
            SecondLevelDescription = createSkillDto.SecondLevelDescription,
            ThirdLevelDescription = createSkillDto.ThirdLevelDescription,
            FourthLevelDescription = createSkillDto.FourthLevelDescription,
        };
    }

    public static SkillDto ToSkillDto(this Skill skill)
    {
        return new SkillDto
        {
            SkillId = skill.SkillId,
            Code = skill.Code,
            Label = skill.Label,
            CriticalityLevel = skill.CriticalityLevel,
            ProjectRequiresSkill = skill.ProjectRequiresSkill,
            RequiredLevel = skill.RequiredLevel,
            FirstLevelDescription = skill.FirstLevelDescription,
            SecondLevelDescription = skill.SecondLevelDescription,
            ThirdLevelDescription = skill.ThirdLevelDescription,
            FourthLevelDescription = skill.FourthLevelDescription,
            CategoryName = skill.Category?.Name ?? string.Empty,
            SubCategoryName = skill.SubCategory?.Name ?? string.Empty,
            CategoryTypeName = skill.Category?.CategoryType?.Name ?? string.Empty,
            CategoryTypeId = skill.Category?.CategoryTypeId ?? 0,
            // Add these two
            CategoryId = skill.CategoryId,
            SubCategoryId = skill.SubCategoryId,
        };
    }
}
