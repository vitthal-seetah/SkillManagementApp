namespace SkillManager.Application.DTOs.Project;

public class UpdateProjectDto
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string ProjectDescription { get; set; } = string.Empty;
}
