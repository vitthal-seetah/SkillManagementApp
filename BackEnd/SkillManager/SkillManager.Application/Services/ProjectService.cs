using SkillManager.Application.DTOs.Project;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Services;

public sealed class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
    {
        var projects = await _projectRepository.GetAllAsync();
        return projects.Select(MapToDto);
    }

    public async Task<ProjectDto?> GetByIdAsync(int projectId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        return project == null ? null : MapToDto(project);
    }

    public async Task<(
        bool Success,
        string Message,
        ProjectDto? CreatedProject
    )> CreateProjectAsync(CreateProjectDto dto)
    {
        var project = new Project
        {
            ProjectName = dto.ProjectName,
            ProjectDescription = dto.ProjectDescription,
        };

        await _projectRepository.AddAsync(project);
        await _projectRepository.SaveChangesAsync();

        return (true, "Project created successfully.", MapToDto(project));
    }

    public async Task<(
        bool Success,
        string Message,
        ProjectDto? UpdatedProject
    )> UpdateProjectAsync(UpdateProjectDto dto)
    {
        var project = await _projectRepository.GetByIdAsync(dto.ProjectId);
        if (project == null)
            return (false, "Project not found.", null);

        project.ProjectName = dto.ProjectName;
        project.ProjectDescription = dto.ProjectDescription;

        await _projectRepository.UpdateAsync(project);
        await _projectRepository.SaveChangesAsync();

        return (true, "Project updated successfully.", MapToDto(project));
    }

    public async Task<bool> DeleteProjectAsync(int projectId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
            return false;

        await _projectRepository.DeleteAsync(project);
        await _projectRepository.SaveChangesAsync();
        return true;
    }

    private static ProjectDto MapToDto(Project p)
    {
        return new ProjectDto
        {
            ProjectId = p.ProjectId,
            ProjectName = p.ProjectName,
            ProjectDescription = p.ProjectDescription,
        };
    }
}
