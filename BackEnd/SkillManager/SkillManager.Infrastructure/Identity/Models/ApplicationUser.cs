using Microsoft.AspNetCore.Identity;

namespace SkillManager.Infrastructure.Identity.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    // ✅ Manually entered unique code (e.g., UT1234)
    public string UTCode { get; set; } = string.Empty;

    // ✅ Manually entered Employee ID (e.g., AO0038)
    public string? EmployeeId { get; set; }
}
