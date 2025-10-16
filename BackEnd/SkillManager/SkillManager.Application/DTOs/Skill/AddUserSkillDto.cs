using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Application.DTOs.Skill;

public class AddUserSkillDto
{
    public int SkillId { get; set; }
    public int Level { get; set; } // 1-4
}
