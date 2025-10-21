using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Domain.Entities
{
    public class Level
    {
        [Key]
        public int LevelId { get; set; }

        [Required]
        public string Name { get; set; } // e.g., Beginner, Intermediate, Expert

        [Required]
        public int Points { get; set; } // Quantitative score

        public virtual ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
    }
}
