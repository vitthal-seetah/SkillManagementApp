using SkillManager.Domain.Entities;
using SkillManager.Domain.Entities.Enums;

namespace SkillManager.Application.DTOs.User;

public class UpdateUserDto
{
    public int UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UtCode { get; set; }
    public string? RefId { get; set; }
    public string? Domain { get; set; }
    public string? Eid { get; set; }
    public string? Status { get; set; } // <-- string
    public string? DeliveryType { get; set; } // <-- string
    public string? RoleName { get; set; }
    public int? ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;

    public int? TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
}
