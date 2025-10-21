using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Entities;

public class User
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UtCode { get; set; }
    public string RefId { get; set; }
    public int RoleId { get; set; }
    public UserStatus Status { get; set; }
    public DeliveryType DeliveryType { get; set; }

    // Navigation properties
    public virtual ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
    public virtual ICollection<UserSME> UserSMEs { get; set; } = new List<UserSME>();
}
