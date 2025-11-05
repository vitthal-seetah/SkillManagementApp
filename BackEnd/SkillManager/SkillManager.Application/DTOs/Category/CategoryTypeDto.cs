namespace SkillManager.Application.DTOs.Category;

public class CategoryTypeDto
{
    public int CategoryTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CategoryCount { get; set; }
}

public class CreateCategoryTypeDto
{
    public string Name { get; set; } = string.Empty;
}

public class UpdateCategoryTypeDto
{
    public string? Name { get; set; }
}
