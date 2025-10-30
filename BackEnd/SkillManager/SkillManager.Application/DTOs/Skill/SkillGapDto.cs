using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Application.DTOs.Skill
{
    public class SkillGapDto
    {
        public int SkillId { get; set; }
        public string SkillCode { get; set; } = string.Empty;
        public string SkillName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int UserLevel { get; set; } // Points
        public string UserLevelName { get; set; } = string.Empty;
        public int RequiredLevel { get; set; } // Points
        public string RequiredLevelCode { get; set; } = string.Empty;
        public string RequiredLevelLabel { get; set; } = string.Empty;

        public bool ProjectRequiresSkill { get; set; }

        public int GapSize => RequiredLevel - UserLevel;
    }
}
