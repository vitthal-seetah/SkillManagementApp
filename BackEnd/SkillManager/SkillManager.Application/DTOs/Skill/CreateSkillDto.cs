namespace SkillManager.Application.DTOs.Skill;

public class CreateSkillDto
{
    public int CategoryId { get; set; }
    public int SubCategoryId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string CriticalityLevel { get; set; } = string.Empty;
    public bool ProjectRequiresSkill { get; set; }
    public int RequiredLevel { get; set; }
    public string FirstLevelDescription { get; set; } = string.Empty;
    public string SecondLevelDescription { get; set; } = string.Empty;
    public string ThirdLevelDescription { get; set; } = string.Empty;
    public string FourthLevelDescription { get; set; } = string.Empty;
}
