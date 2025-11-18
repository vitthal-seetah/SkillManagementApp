using Moq;
using SkillManager.Application.DTOs.Project;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Application.Services;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.UnitTest.Tests;

public class ProjectServiceTests
{
    private readonly Mock<IProjectRepository> _mockRepo;
    private readonly ProjectService _service;

    public ProjectServiceTests()
    {
        _mockRepo = new Mock<IProjectRepository>();
        _service = new ProjectService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllProjectsAsync_ReturnsAllProjects()
    {
        var projects = new List<Project>
        {
            new Project
            {
                ProjectId = 1,
                ProjectName = "P1",
                ProjectDescription = "Desc1",
            },
            new Project
            {
                ProjectId = 2,
                ProjectName = "P2",
                ProjectDescription = "Desc2",
            },
        };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(projects);

        var result = await _service.GetAllProjectsAsync();

        Assert.Equal(2, result.Count());
        Assert.Contains(result, r => r.ProjectName == "P1");
        Assert.Contains(result, r => r.ProjectName == "P2");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsProject_WhenExists()
    {
        var project = new Project
        {
            ProjectId = 1,
            ProjectName = "P1",
            ProjectDescription = "Desc1",
        };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(project);

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("P1", result.ProjectName);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Project?)null);

        var result = await _service.GetByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateProjectAsync_ReturnsSuccessAndProjectDto()
    {
        var dto = new CreateProjectDto { ProjectName = "P1", ProjectDescription = "Desc1" };

        _mockRepo.Setup(r => r.AddAsync(It.IsAny<Project>())).Returns(Task.CompletedTask);
        _mockRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var result = await _service.CreateProjectAsync(dto);

        Assert.True(result.Success);
        Assert.Equal("Project created successfully.", result.Message);
        Assert.NotNull(result.CreatedProject);
        Assert.Equal("P1", result.CreatedProject.ProjectName);

        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Once);
        _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateProjectAsync_ReturnsFailure_WhenProjectNotFound()
    {
        var dto = new UpdateProjectDto
        {
            ProjectId = 1,
            ProjectName = "Updated",
            ProjectDescription = "Desc",
        };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Project?)null);

        var result = await _service.UpdateProjectAsync(dto);

        Assert.False(result.Success);
        Assert.Equal("Project not found.", result.Message);
        Assert.Null(result.UpdatedProject);
    }

    [Fact]
    public async Task UpdateProjectAsync_ReturnsSuccess_WhenProjectExists()
    {
        var existing = new Project
        {
            ProjectId = 1,
            ProjectName = "Old",
            ProjectDescription = "OldDesc",
        };
        var dto = new UpdateProjectDto
        {
            ProjectId = 1,
            ProjectName = "New",
            ProjectDescription = "NewDesc",
        };

        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _mockRepo.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);
        _mockRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var result = await _service.UpdateProjectAsync(dto);

        Assert.True(result.Success);
        Assert.Equal("Project updated successfully.", result.Message);
        Assert.NotNull(result.UpdatedProject);
        Assert.Equal("New", result.UpdatedProject.ProjectName);
        Assert.Equal("NewDesc", result.UpdatedProject.ProjectDescription);

        _mockRepo.Verify(r => r.UpdateAsync(existing), Times.Once);
        _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteProjectAsync_ReturnsFalse_WhenProjectNotFound()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Project?)null);

        var result = await _service.DeleteProjectAsync(1);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteProjectAsync_ReturnsTrue_WhenProjectDeleted()
    {
        var existing = new Project
        {
            ProjectId = 1,
            ProjectName = "P1",
            ProjectDescription = "Desc",
        };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _mockRepo.Setup(r => r.DeleteAsync(existing)).Returns(Task.CompletedTask);
        _mockRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var result = await _service.DeleteProjectAsync(1);

        Assert.True(result);
        _mockRepo.Verify(r => r.DeleteAsync(existing), Times.Once);
        _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
