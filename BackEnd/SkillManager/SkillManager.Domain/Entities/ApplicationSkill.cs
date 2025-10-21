using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Domain.Entities;

public class ApplicationSkill
{
    public int ApplicationId { get; set; }
    public int SkillId { get; set; }

    // Navigation properties
    public virtual Application Application { get; set; }
    public virtual Skill Skill { get; set; }
}
