namespace SkillManager.Infrastructure.DTOs.Skill;

public class UpdateUserSkillsDto
{
    public int SkillId { get; set; }
    public int LevelId { get; set; }
    public DateTime UpdatedTime { get; set; } = DateTime.UtcNow;
    public bool ApprovedSkill { get; set; }
}
