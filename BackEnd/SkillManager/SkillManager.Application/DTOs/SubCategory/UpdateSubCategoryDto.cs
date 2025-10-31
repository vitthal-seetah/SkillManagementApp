using System.ComponentModel.DataAnnotations;

namespace SkillManager.Application.DTOs.SubCategory
{
    public class UpdateSubCategoryDto
    {
        [StringLength(100)]
        public string? Name { get; set; }

        public int? CategoryId { get; set; }
    }
}
