namespace SkillManager.Domain.Entities;

public class ApplicationSkill
{
    public int ApplicationId { get; set; }
    public int SkillId { get; set; }

    // Navigation properties
    public virtual Application Application { get; set; }
    public virtual Skill Skill { get; set; }
}
