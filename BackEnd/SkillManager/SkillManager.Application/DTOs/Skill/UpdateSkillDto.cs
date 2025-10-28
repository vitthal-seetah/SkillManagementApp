using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Application.DTOs.Skill
{
    public class UpdateSkillDto
    {
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string? Code { get; set; }
        public string? Label { get; set; }
        public string? CriticalityLevel { get; set; }
        public bool? ProjectRequiresSkill { get; set; }
        public int? RequiredLevel { get; set; }
        public string? FirstLevelDescription { get; set; }
        public string? SecondLevelDescription { get; set; }
        public string? ThirdLevelDescription { get; set; }
        public string? FourthLevelDescription { get; set; }
    }
}
