using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Application.DTOs.Level
{
    public class UpdateLevelDto
    {
        public string Name { get; set; } = string.Empty;
        public int? Points { get; set; }
    }
}
