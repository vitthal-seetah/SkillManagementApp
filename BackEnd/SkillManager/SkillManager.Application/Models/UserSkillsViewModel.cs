using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Application.Models
{
    public class UserSkillsViewModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int SkillId { get; set; }
        public string SkillCode { get; set; } = string.Empty;
        public string SkillLabel { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string LevelName { get; set; } = string.Empty;
        public int LevelPoints { get; set; }
        public int LevelId { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
