using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Infrastructure.DTOs.Skill;

public class AddUserSkillDto
{
    public int SkillId { get; set; }
    public int Level { get; set; } // 1-4
}
