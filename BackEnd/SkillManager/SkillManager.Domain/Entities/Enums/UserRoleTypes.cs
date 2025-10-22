using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Domain.Entities.Enums
{
    public enum UserRoleTypes
    {
        Admin = 1, // AccessLevel: 1
        TechLead = 2, // AccessLevel: 2
        Manager = 3, // AccessLevel: 3
        Employee = 4, // AccessLevel: 4
    }
}
