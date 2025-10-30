using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Application.DTOs.Skill;

namespace SkillManager.Application.Models
{
    public class CategoryNavigationViewModel
    {
        public List<CategoryTypeWithCategories> CategoryTypes { get; set; } = new();
        public int? SelectedCategoryId { get; set; }
        public List<UserSkillsWithLevels>? UserSkills { get; set; }
    }
}
