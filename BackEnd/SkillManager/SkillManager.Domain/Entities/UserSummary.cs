namespace SkillManager.Domain.Entities;

public class UserSummary
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UtCode { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public string TeamName { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public Dictionary<string, double> CategoryAverages { get; set; } = new();
    public int TotalSkills { get; set; }
    public double OverallAverage { get; set; }
    public DateTime? LastUpdated { get; set; }
}
