using SkillManager.Application.DTOs.SubCategory;

namespace SkillManager.Application.DTOs.Category
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public int CategoryTypeId { get; set; }

        // Name of the CategoryType (Functional, Technical, etc.)
        public string CategoryTypeName { get; set; } = string.Empty;

        // Optional — if you need to show subcategories in UI
        public List<SubCategoryDto> SubCategories { get; set; } = new();
    }
}
