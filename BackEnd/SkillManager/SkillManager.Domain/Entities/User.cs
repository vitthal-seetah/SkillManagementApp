using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Application.Entities;

public class User
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UTCode { get; set; } = string.Empty;
    public string? EmployeeId { get; set; }
    public string Email { get; set; } = string.Empty;
}
