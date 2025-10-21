using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Domain.Enums;

public enum UserStatus
{
    Active = 1,
    Departed = 2,
    Cancelled = 3,
}

public enum DeliveryType
{
    Onshore = 1,
    Offshore = 2,
}
