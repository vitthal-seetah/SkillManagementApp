using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Infrastructure.DTOs.Skill;

public class UserSkillDto
{
    public int UserId { get; set; }
    public int SkillId { get; set; }
    public string SkillName { get; set; }
    public string SkillCode { get; set; }
    public string CategoryName { get; set; }
    public string CategoryType { get; set; }
    public int LevelId { get; set; }
    public string LevelName { get; set; }
}
