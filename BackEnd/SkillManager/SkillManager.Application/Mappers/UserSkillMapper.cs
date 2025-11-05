using SkillManager.Application.Models;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Mappers;

public static class UserSkillMapper
{
    public static UserSkillsWithLevels ToUserSkillsWithLevels(this UserSkill userSkill)
    {
        if (userSkill == null)
            return null;

        return new UserSkillsWithLevels
        {
            SkillId = userSkill.SkillId,
            Code = userSkill.Skill?.Code ?? string.Empty, // From Skill navigation property
            Label = userSkill.Skill?.Label ?? string.Empty,
            RequiredLevel = userSkill.Skill?.RequiredLevel.ToString() ?? "0", // From Skill
            LevelId = userSkill.LevelId,
            LevelName = userSkill.Level?.Name ?? "Not Set", // From Level navigation property
        };
    }
}
