using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillManager.Domain.Entities;

public class Skill
{
    public int SkillId { get; set; }
    public int CategoryId { get; set; }
    public int SubCategoryId { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Label { get; set; }
    public string CriticalityLevel { get; set; }
    public bool ProjectRequiresSkill { get; set; }
    public int RequiredLevel { get; set; }
    public string FirstLevelDescription { get; set; }
    public string SecondLevelDescription { get; set; }
    public string ThirdLevelDescription { get; set; }
    public string FourthLevelDescription { get; set; }

    // Navigation properties
    public virtual Category Category { get; set; }
    public virtual SubCategory SubCategory { get; set; }
    public virtual ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
    public virtual ICollection<UserSME> UserSMEs { get; set; } = new List<UserSME>();
    public virtual ICollection<ApplicationSkill> ApplicationSkills { get; set; } =
        new List<ApplicationSkill>();
}
