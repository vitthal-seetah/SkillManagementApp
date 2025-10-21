using System.ComponentModel.DataAnnotations;

namespace SkillManager.Domain.Entities;

public class Application
{
    [Key]
    public int ApplicationId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty!;

    // Foreign keys
    public int SuiteId { get; set; }
    public int CategoryId { get; set; }

    public virtual ApplicationSuite ApplicationSuite { get; set; }
    public virtual Category Category { get; set; }
    public virtual ICollection<ApplicationSkill> ApplicationSkills { get; set; } =
        new List<ApplicationSkill>();
}
