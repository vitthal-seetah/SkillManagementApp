using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Application.Models
{
    public class UserSkillsWithLevels
    {
        public int SkillId { get; set; }

        public string Code { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string RequiredLevel { get; set; } = string.Empty;
        public int LevelId { get; set; }
        public string LevelName { get; set; } = string.Empty;

        public DateTime UpdatedTime { get; set; }
    }
}
