namespace SkillManager.Domain.Entities;

public class UserSkill
{
    public string UserId { get; set; }
    public int SkillId { get; set; }
    public int LevelId { get; set; }

    // Navigation properties
    public virtual Skill Skill { get; set; }
    public virtual Level Level { get; set; }
}
