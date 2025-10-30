using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Application.DTOs.Skill;

namespace SkillManager.Application.DTOs.Category
{
    public class CategoryGapDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public List<SkillGapDto> Skills { get; set; } = new();
        public int AverageGap { get; set; }
    }
}
