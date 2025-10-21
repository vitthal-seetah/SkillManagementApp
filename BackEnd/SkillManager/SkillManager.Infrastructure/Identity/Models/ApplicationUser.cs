using Microsoft.AspNetCore.Identity;
using SkillManager.Domain.Enums;

namespace SkillManager.Infrastructure.Identity.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UTCode { get; set; } = string.Empty;
    public string RefId { get; set; } = string.Empty;

    // Business-related fields
    public UserStatus Status { get; set; } = UserStatus.Active;
    public DeliveryType DeliveryType { get; set; } = DeliveryType.Onshore;

    // Optional if you store a numeric role mapping in addition to Identity roles
    public int? RoleId { get; set; }
}
