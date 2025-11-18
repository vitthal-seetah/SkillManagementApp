using Moq;
using SkillManager.Application.DTOs.Team;
using SkillManager.Application.DTOs.User;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Application.Services;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.UnitTest.Tests;

public class TeamServiceTests
{
    private readonly Mock<ITeamRepository> _mockTeamRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<IProjectRepository> _mockProjectRepo;
    private readonly TeamService _service;

    public TeamServiceTests()
    {
        _mockTeamRepo = new Mock<ITeamRepository>();
        _mockUserRepo = new Mock<IUserRepository>();
        _mockProjectRepo = new Mock<IProjectRepository>();
        _service = new TeamService(
            _mockTeamRepo.Object,
            _mockUserRepo.Object,
            _mockProjectRepo.Object
        );
    }

    [Fact]
    public async Task GetTeamByIdAsync_ReturnsTeam()
    {
        var team = new Team { TeamId = 1, TeamName = "Team1" };
        _mockTeamRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(team);

        var result = await _service.GetTeamByIdAsync(1);

        Assert.Equal("Team1", result!.TeamName);
    }

    [Fact]
    public async Task GetAllTeamsAsync_ReturnsTeams()
    {
        var teams = new List<Team> { new Team(), new Team() };
        _mockTeamRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(teams);

        var result = await _service.GetAllTeamsAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CreateTeamAsync_Throws_WhenProjectMissing()
    {
        var dto = new CreateTeamDto { ProjectId = 1, TeamName = "T1" };
        _mockProjectRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Project?)null);

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateTeamAsync(dto));
    }

    [Fact]
    public async Task CreateTeamAsync_Throws_WhenTeamLeadMissing()
    {
        var dto = new CreateTeamDto
        {
            ProjectId = 1,
            TeamLeadId = 5,
            TeamName = "T1",
        };
        _mockProjectRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Project());
        _mockUserRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateTeamAsync(dto));
    }

    [Fact]
    public async Task CreateTeamAsync_ReturnsTeam()
    {
        var dto = new CreateTeamDto { ProjectId = 1, TeamName = "T1" };
        var createdTeam = new Team { TeamId = 1, TeamName = "T1" };

        _mockProjectRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Project());
        _mockTeamRepo.Setup(r => r.AddAsync(It.IsAny<Team>())).ReturnsAsync(createdTeam);
        _mockTeamRepo.Setup(r => r.AddTeamToProjectAsync(1, 1)).Returns(Task.CompletedTask);

        var result = await _service.CreateTeamAsync(dto);

        Assert.Equal("T1", result.TeamName);
        Assert.Equal(1, result.TeamId);
    }

    [Fact]
    public async Task DeleteTeamAsync_Throws_WhenTeamHasMembers()
    {
        var team = new Team { TeamId = 1 };
        _mockTeamRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(team);
        _mockTeamRepo
            .Setup(r => r.GetTeamMembersAsync(team))
            .ReturnsAsync(new List<User> { new User() });

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.DeleteTeamAsync(1));
    }

    [Fact]
    public async Task DeleteTeamAsync_ReturnsTrue_WhenNoMembers()
    {
        var team = new Team { TeamId = 1 };
        _mockTeamRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(team);
        _mockTeamRepo.Setup(r => r.GetTeamMembersAsync(team)).ReturnsAsync(new List<User>());
        _mockTeamRepo.Setup(r => r.DeleteAsync(team)).ReturnsAsync(true);

        var result = await _service.DeleteTeamAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task AddMemberToTeamAsync_Throws_WhenAlreadyInAnotherTeam()
    {
        var team = new Team { TeamId = 1 };
        var user = new User { UserId = 5, TeamId = 2 };
        _mockTeamRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(team);
        _mockUserRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(user);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.AddMemberToTeamAsync(1, new UserDto { UserId = 5 })
        );
    }

    [Fact]
    public async Task AddMemberToTeamAsync_ReturnsTrue_WhenValid()
    {
        var team = new Team { TeamId = 1 };
        var user = new User { UserId = 5, TeamId = null };
        _mockTeamRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(team);
        _mockUserRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(user);
        _mockTeamRepo.Setup(r => r.AddUserToTeamAsync(team, user)).ReturnsAsync(true);

        var result = await _service.AddMemberToTeamAsync(1, new UserDto { UserId = 5 });

        Assert.True(result);
    }

    [Fact]
    public async Task RemoveMemberFromTeamAsync_Throws_WhenUserIsTeamLead()
    {
        var team = new Team { TeamId = 1, TeamLeadId = 5 };
        var user = new User { UserId = 5 };
        _mockTeamRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(team);
        _mockUserRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(user);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.RemoveMemberFromTeamAsync(1, new UserDto { UserId = 5 })
        );
    }

    [Fact]
    public async Task SetTeamLeadAsync_Throws_WhenUserNotMember()
    {
        var team = new Team { TeamId = 1 };
        _mockTeamRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(team);
        _mockUserRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(new User());
        _mockTeamRepo.Setup(r => r.GetTeamMembersAsync(team)).ReturnsAsync(new List<User>());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.SetTeamLeadAsync(1, new UserDto { UserId = 5 })
        );
    }

    [Fact]
    public async Task GetTeamsByProjectIdAsync_Throws_WhenInvalidProjectId()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _service.GetTeamsByProjectIdAsync(0));
    }

    [Fact]
    public async Task GetUserTeamAsync_ReturnsNull_WhenNoTeam()
    {
        var user = new User { UserId = 1, TeamId = null };

        var result = await _service.GetUserTeamAsync(user);

        Assert.Null(result);
    }

    [Fact]
    public async Task IsUserInTeamAsync_ReturnsTrue_WhenInTeam()
    {
        var team = new Team { TeamId = 1 };
        var user = new User { TeamId = 1 };

        var result = await _service.IsUserInTeamAsync(user, team);

        Assert.True(result);
    }

    [Fact]
    public async Task GetTeamProjectIdAsync_Returns0_WhenNoProject()
    {
        _mockTeamRepo
            .Setup(r => r.GetTeamWithProjectsAsync(1))
            .ReturnsAsync(new Team { ProjectTeams = new List<ProjectTeam>() });

        var result = await _service.GetTeamProjectIdAsync(1);

        Assert.Equal(0, result);
    }
}
