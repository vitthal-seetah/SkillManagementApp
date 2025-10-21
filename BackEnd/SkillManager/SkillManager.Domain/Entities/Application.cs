using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Domain.Entities;

public class Application
{
    [Key]
    public int ApplicationId { get; set; }

    [Required]
    public string Name { get; set; }

    // Foreign keys
    public int SuiteId { get; set; }
    public int CategoryId { get; set; }

    public virtual ApplicationSuite ApplicationSuite { get; set; }
    public virtual Category Category { get; set; }
    public virtual ICollection<ApplicationSkill> ApplicationSkills { get; set; } =
        new List<ApplicationSkill>();
}
