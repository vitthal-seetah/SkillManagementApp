using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Domain.Entities.Enums;

namespace SkillManager.Domain.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        public string UtCode { get; set; }

        [MaxLength(100)]
        public string RefId { get; set; }

        public int RoleId { get; set; }
        public virtual UserRole Role { get; set; }

        public int? ProjectId { get; set; }

        public virtual Project? Project { get; set; }

        public int? TeamId { get; set; }
        public virtual Team? Team { get; set; }

        [Required]
        public string Domain { get; set; } = string.Empty;

        [Required]
        public string Eid { get; set; }
        public UserStatus Status { get; set; }

        public DeliveryType DeliveryType { get; set; }

        public virtual ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
        public virtual ICollection<UserSME> UserSMEs { get; set; } = new List<UserSME>();
    }
}
