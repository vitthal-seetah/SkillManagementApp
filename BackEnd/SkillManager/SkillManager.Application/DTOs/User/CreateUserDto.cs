using SkillManager.Domain.Entities;
using SkillManager.Domain.Entities.Enums;

namespace SkillManager.Application.DTOs.User;

public class CreateUserDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UtCode { get; set; } = string.Empty;
    public string RefId { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public string Eid { get; set; } = string.Empty;

    public string? Status { get; set; } // <-- string
    public string? DeliveryType { get; set; } // <-- string
    public string RoleName { get; set; } = string.Empty;
    public int? ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;

    public int? TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
}
