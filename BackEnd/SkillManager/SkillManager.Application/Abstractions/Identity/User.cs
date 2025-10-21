using SkillManager.Domain.Enums;

namespace SkillManager.Application.Abstractions.Identity;

public sealed class User
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UTCode { get; set; } = string.Empty;
    public string RefId { get; set; } = string.Empty;
    public int? RoleId { get; set; }
    public UserStatus Status { get; set; }
    public DeliveryType DeliveryType { get; set; }
}
