using System.ComponentModel.DataAnnotations;
using SkillManager.Application.Entities;

namespace SkillManager.Domain.Entities;

public class UserSkill
{
    public int UserId { get; set; }
    public int SkillId { get; set; }
    public int LevelId { get; set; }

    // Navigation properties
    public virtual User User { get; set; }
    public virtual Skill Skill { get; set; }
    public virtual Level Level { get; set; }
}
