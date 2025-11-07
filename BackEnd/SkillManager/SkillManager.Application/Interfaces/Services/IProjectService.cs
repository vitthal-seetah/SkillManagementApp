using SkillManager.Application.DTOs.Project;

namespace SkillManager.Application.Interfaces.Services;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
    Task<ProjectDto?> GetByIdAsync(int projectId);
    Task<(bool Success, string Message, ProjectDto? CreatedProject)> CreateProjectAsync(
        CreateProjectDto dto
    );
    Task<(bool Success, string Message, ProjectDto? UpdatedProject)> UpdateProjectAsync(
        UpdateProjectDto dto
    );
    Task<bool> DeleteProjectAsync(int projectId);
}
