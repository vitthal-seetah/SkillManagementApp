using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Application.Entities;

namespace SkillManager.Domain.Entities
{
    public class UserSME
    {
        public int UserId { get; set; }
        public int SkillId { get; set; }
        public int CategoryTypeId { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Skill Skill { get; set; }
        public virtual CategoryType CategoryType { get; set; }
    }
}
