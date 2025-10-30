using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Application.DTOs.Category;

namespace SkillManager.Application.Models
{
    public class CategoryTypeWithCategories
    {
        public int CategoryTypeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<CategoryDto> Categories { get; set; } = new();
    }
}
