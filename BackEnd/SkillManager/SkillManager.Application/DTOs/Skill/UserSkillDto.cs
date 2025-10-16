using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Application.DTOs.Skill;

public class UserSkillDto
{
    public int Id { get; set; }
    public string SkillName { get; set; } = default!;
    public string SkillCode { get; set; } = default!;
    public string SectionName { get; set; } = default!;
    public string CategoryName { get; set; } = default!;
    public int Level { get; set; }
}
