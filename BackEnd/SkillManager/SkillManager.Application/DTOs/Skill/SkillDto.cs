using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Application.DTOs.Category;
using SkillManager.Application.DTOs.SubCategory;

namespace SkillManager.Application.DTOs.Skill;

public class SkillDto
{
    public int SkillId { get; set; }

    public string Code { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string CriticalityLevel { get; set; } = string.Empty;
    public bool ProjectRequiresSkill { get; set; }
    public int RequiredLevel { get; set; }
    public string FirstLevelDescription { get; set; } = string.Empty;
    public string SecondLevelDescription { get; set; } = string.Empty;
    public string ThirdLevelDescription { get; set; } = string.Empty;
    public string FourthLevelDescription { get; set; } = string.Empty;

    public string CategoryName { get; set; } = string.Empty;
    public string SubCategoryName { get; set; } = string.Empty;

    // Category Type information
    public string CategoryTypeName { get; set; } = string.Empty;
    public int CategoryTypeId { get; set; }
    public int CategoryId { get; set; }
    public int SubCategoryId { get; set; }
}
