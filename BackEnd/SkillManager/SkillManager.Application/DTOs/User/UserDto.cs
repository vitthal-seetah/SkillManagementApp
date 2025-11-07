using SkillManager.Domain.Entities;
using SkillManager.Domain.Entities.Enums;

namespace SkillManager.Application.DTOs.User;

public class UserDto
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string UtCode { get; set; } = string.Empty;

    public string RefId { get; set; } = string.Empty;

    public string RoleName { get; set; } = string.Empty; // Role.Name

    public string Domain { get; set; } = string.Empty;

    public string Eid { get; set; } = string.Empty;

    public string TeamName { get; set; } = string.Empty;

    public int TeamId { get; set; }
    public UserStatus Status { get; set; }
    public DeliveryType DeliveryType { get; set; }
    public int? ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;

    public int? TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
}
