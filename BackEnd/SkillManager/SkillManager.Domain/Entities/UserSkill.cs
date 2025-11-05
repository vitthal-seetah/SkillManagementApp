namespace SkillManager.Domain.Entities;

public class UserSkill
{
    public int UserId { get; set; }
    public int SkillId { get; set; }
    public int LevelId { get; set; }
    public DateTime UpdatedTime { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Skill Skill { get; set; }
    public virtual Level Level { get; set; }
    public virtual User User { get; set; }
}
