using System.ComponentModel.DataAnnotations;
using SkillManager.Infrastructure.Identity.Models;

namespace SkillManager.Domain.Entities;

public class UserSkill
{
    public int UserId { get; set; }
    public int SkillId { get; set; }
    public int LevelId { get; set; }

    // Navigation properties
    public virtual Skill Skill { get; set; }
    public virtual Level Level { get; set; }
}
