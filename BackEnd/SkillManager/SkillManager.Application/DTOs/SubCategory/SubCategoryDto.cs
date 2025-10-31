namespace SkillManager.Application.DTOs.SubCategory
{
    public class SubCategoryDto
    {
        public int SubCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int SkillCount { get; set; }
    }
}
