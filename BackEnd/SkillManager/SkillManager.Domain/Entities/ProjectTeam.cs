using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Domain.Entities;

public class ProjectTeam
{
    public int ProjectId { get; set; }
    public int TeamId { get; set; }

    public virtual Project? Project { get; set; }
    public virtual Team? Team { get; set; }
}
