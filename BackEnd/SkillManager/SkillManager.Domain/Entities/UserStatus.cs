using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Domain.Entities
{
    public enum UserStatus
    {
        Active,
        Departed, // Fixed typo from "Desarted"
        Cancelled, // Fixed typo from "Canceled"
    }
}
