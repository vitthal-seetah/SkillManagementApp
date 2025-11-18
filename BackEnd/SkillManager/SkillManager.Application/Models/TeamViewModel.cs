namespace SkillManager.Application.Models;

public class TeamViewModel
{
    public int TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public string TeamDescription { get; set; } = string.Empty;
    public int TeamLeadId { get; set; }
    public int MemberCount { get; set; }
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
}
