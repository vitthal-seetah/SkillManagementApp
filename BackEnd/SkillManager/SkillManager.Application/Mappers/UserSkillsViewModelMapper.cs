using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Application.Models;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Mappers
{
    public static class UserSkillsViewModelMapper
    {
        public static UserSkillsViewModel ToUserSkillsViewModel(this UserSkill userSkill)
        {
            return new UserSkillsViewModel
            {
                UserId = userSkill.UserId,
                FirstName = userSkill.User?.FirstName ?? string.Empty,
                LastName = userSkill.User?.LastName ?? string.Empty,
                SkillId = userSkill.SkillId,
                SkillCode = userSkill.Skill?.Code ?? string.Empty,
                SkillLabel = userSkill.Skill?.Label ?? string.Empty,
                CategoryId = userSkill.Skill?.CategoryId ?? 0,
                CategoryName = userSkill.Skill?.Category?.Name ?? string.Empty,
                LevelName = userSkill.Level?.Name ?? string.Empty,
                LevelPoints = userSkill.Level?.Points ?? 0,
                LevelId = userSkill.LevelId,
            };
        }
    }
}
