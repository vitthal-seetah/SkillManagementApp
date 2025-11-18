using FluentValidation;
using FluentValidation.Results;
using Moq;
using SkillManager.Application.DTOs.User;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Application.Services;
using SkillManager.Domain.Entities;
using SkillManager.Domain.Entities.Enums;
using Xunit;

namespace SkillManager.Application.UnitTest.Tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repo;
    private readonly Mock<IValidator<CreateUserDto>> _createValidator;
    private readonly Mock<IValidator<UpdateUserDto>> _updateValidator;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _repo = new Mock<IUserRepository>();
        _createValidator = new Mock<IValidator<CreateUserDto>>();
        _updateValidator = new Mock<IValidator<UpdateUserDto>>();

        _createValidator
            .Setup(v => v.ValidateAsync(It.IsAny<CreateUserDto>(), default))
            .ReturnsAsync(new ValidationResult());

        _updateValidator
            .Setup(v => v.ValidateAsync(It.IsAny<UpdateUserDto>(), default))
            .ReturnsAsync(new ValidationResult());

        _service = new UserService(_repo.Object, _createValidator.Object, _updateValidator.Object);
    }

    private User CreateCurrentUser(int projectId = 1, int teamId = 1) =>
        new User
        {
            UserId = 999,
            ProjectId = projectId,
            TeamId = teamId,
        };

    // -------------------------------------------------------------------
    // GET ALL USERS
    // -------------------------------------------------------------------
    [Fact]
    public async Task GetAllAsync_ReturnsOnlyProjectUsers()
    {
        var current = CreateCurrentUser(1);

        var users = new List<User>
        {
            new User { UserId = 1, ProjectId = 1 },
            new User { UserId = 2, ProjectId = 1 },
        };

        _repo.Setup(r => r.GetByProjectIdAsync(1)).ReturnsAsync(users);

        var result = (await _service.GetAllAsync(current)).ToList();

        Assert.Equal(2, result.Count);
    }

    // -------------------------------------------------------------------
    // GET ALL USERS BY TEAM
    // -------------------------------------------------------------------
    [Fact]
    public async Task GetAllByTeamAsync_ReturnsUsersFromSameTeam()
    {
        var current = CreateCurrentUser(1, 10);

        var users = new List<User>
        {
            new User
            {
                UserId = 1,
                ProjectId = 1,
                TeamId = 10,
            },
        };

        _repo.Setup(r => r.GetByProjectAndTeamAsync(current)).ReturnsAsync(users);

        var result = (await _service.GetAllByTeamAsync(current)).ToList();

        Assert.Single(result);
    }

    // -------------------------------------------------------------------
    // GET USER BY ID WITH PROJECT CHECK
    // -------------------------------------------------------------------
    [Fact]
    public async Task GetUserByIdAsync_ReturnsNull_WhenDifferentProject()
    {
        var current = CreateCurrentUser(1);
        var user = new User { UserId = 5, ProjectId = 77 }; // different project

        _repo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(user);

        var result = await _service.GetUserByIdAsync(5, current);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByIdAsync_ReturnsDto_WhenSameProject()
    {
        var current = CreateCurrentUser(1);
        var user = new User { UserId = 5, ProjectId = 1 };

        _repo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(user);

        var result = await _service.GetUserByIdAsync(5, current);

        Assert.NotNull(result);
        Assert.Equal(5, result!.UserId);
    }

    // -------------------------------------------------------------------
    // GET BY ID (no project rule)
    // -------------------------------------------------------------------
    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        _repo.Setup(r => r.GetByIdAsync(9)).ReturnsAsync((User?)null);

        var result = await _service.GetByIdAsync(9);

        Assert.Null(result);
    }

    // -------------------------------------------------------------------
    // CREATE USER
    // -------------------------------------------------------------------
    [Fact]
    public async Task CreateUserAsync_ReturnsError_WhenValidationFails()
    {
        var dto = new CreateUserDto();

        var bad = new ValidationResult(new[] { new ValidationFailure("FirstName", "Required") });

        _createValidator
            .Setup(v => v.ValidateAsync(It.IsAny<CreateUserDto>(), default))
            .ReturnsAsync(bad);

        var result = await _service.CreateUserAsync(dto, CreateCurrentUser(1));

        Assert.False(result.Success);
        Assert.Equal("Required", result.Message);
    }

    [Fact]
    public async Task CreateUserAsync_ReturnsError_WhenDuplicateUtCode()
    {
        var dto = new CreateUserDto
        {
            FirstName = "A",
            LastName = "B",
            UtCode = "UT123",
            ProjectId = 1,
        };

        _repo.Setup(r => r.GetByUtCodeAsync("UT123")).ReturnsAsync(new User());

        var result = await _service.CreateUserAsync(dto, CreateCurrentUser(1));

        Assert.False(result.Success);
        Assert.Contains("already exists", result.Message);
    }

    [Fact]
    public async Task CreateUserAsync_ReturnsError_WhenRoleNotFound()
    {
        var dto = new CreateUserDto
        {
            FirstName = "A",
            LastName = "B",
            UtCode = "UT123",
            ProjectId = 1,
            RoleName = "Manager",
        };

        _repo.Setup(r => r.GetRoleByNameAsync("Manager")).ReturnsAsync((UserRole?)null);

        var result = await _service.CreateUserAsync(dto, CreateCurrentUser(1));

        Assert.False(result.Success);
    }

    [Fact]
    public async Task CreateUserAsync_ReturnsError_WhenProjectIdDifferent()
    {
        var dto = new CreateUserDto
        {
            FirstName = "A",
            LastName = "B",
            UtCode = "UT123",
            ProjectId = 99, // different project
            RoleName = "Admin",
        };

        _repo.Setup(r => r.GetRoleByNameAsync("Admin")).ReturnsAsync(new UserRole { RoleId = 1 });

        var result = await _service.CreateUserAsync(dto, CreateCurrentUser(1));

        Assert.False(result.Success);
        Assert.Contains("within your own project", result.Message);
    }

    [Fact]
    public async Task CreateUserAsync_Success()
    {
        var dto = new CreateUserDto
        {
            FirstName = "A",
            LastName = "B",
            UtCode = "UT123",
            ProjectId = 1,
            RoleName = "Admin",
        };

        _repo.Setup(r => r.GetRoleByNameAsync("Admin")).ReturnsAsync(new UserRole { RoleId = 10 });

        _repo.Setup(r => r.GetByUtCodeAsync("UT123")).ReturnsAsync((User?)null);

        var result = await _service.CreateUserAsync(dto, CreateCurrentUser(1));

        Assert.True(result.Success);
        Assert.NotNull(result.CreatedUser);
    }

    // -------------------------------------------------------------------
    // UPDATE USER
    // -------------------------------------------------------------------
    [Fact]
    public async Task UpdateUserAsync_ReturnsError_WhenValidationFails()
    {
        var dto = new UpdateUserDto { UserId = 1 };

        var bad = new ValidationResult(new[] { new ValidationFailure("FirstName", "Required") });

        _updateValidator
            .Setup(v => v.ValidateAsync(It.IsAny<UpdateUserDto>(), default))
            .ReturnsAsync(bad);

        var result = await _service.UpdateUserAsync(dto, CreateCurrentUser(1));

        Assert.False(result.Success);
        Assert.Equal("Required", result.Message);
    }

    [Fact]
    public async Task UpdateUserAsync_ReturnsError_WhenUserNotFound()
    {
        _repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((User?)null);

        var dto = new UpdateUserDto { UserId = 1 };

        var result = await _service.UpdateUserAsync(dto, CreateCurrentUser(1));

        Assert.False(result.Success);
        Assert.Equal("User not found.", result.Message);
    }

    [Fact]
    public async Task UpdateUserAsync_ReturnsError_WhenProjectMismatch()
    {
        var user = new User { UserId = 1, ProjectId = 99 };

        _repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

        var dto = new UpdateUserDto { UserId = 1 };

        var result = await _service.UpdateUserAsync(dto, CreateCurrentUser(1));

        Assert.False(result.Success);
    }

    [Fact]
    public async Task UpdateUserAsync_ReturnsSuccess()
    {
        var user = new User
        {
            UserId = 1,
            ProjectId = 1,
            UtCode = "Old",
        };

        _repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
        _repo.Setup(r => r.GetByUtCodeAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

        var dto = new UpdateUserDto
        {
            UserId = 1,
            FirstName = "New FN",
            LastName = "New LN",
        };

        var result = await _service.UpdateUserAsync(dto, CreateCurrentUser(1));

        Assert.True(result.Success);
    }

    // -------------------------------------------------------------------
    // UPDATE USER ROLE
    // -------------------------------------------------------------------
    [Fact]
    public async Task UpdateUserRoleAsync_ReturnsFalse_WhenRoleNotFound()
    {
        _repo.Setup(r => r.GetRoleByNameAsync("Admin")).ReturnsAsync((UserRole?)null);

        var result = await _service.UpdateUserRoleAsync(1, "Admin", CreateCurrentUser(1));

        Assert.False(result);
    }

    [Fact]
    public async Task UpdateUserRoleAsync_ReturnsFalse_WhenUserDifferentProject()
    {
        var role = new UserRole { RoleId = 5 };
        var user = new User { UserId = 1, ProjectId = 2 };

        _repo.Setup(r => r.GetRoleByNameAsync("Admin")).ReturnsAsync(role);
        _repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

        var result = await _service.UpdateUserRoleAsync(1, "Admin", CreateCurrentUser(1));

        Assert.False(result);
    }

    [Fact]
    public async Task UpdateUserRoleAsync_Success()
    {
        var role = new UserRole { RoleId = 5 };
        var user = new User
        {
            UserId = 1,
            ProjectId = 1,
            RoleId = 2,
        };

        _repo.Setup(r => r.GetRoleByNameAsync("Admin")).ReturnsAsync(role);
        _repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

        var result = await _service.UpdateUserRoleAsync(1, "Admin", CreateCurrentUser(1));

        Assert.True(result);
    }

    // -------------------------------------------------------------------
    // GET BY DOMAIN + EID
    // -------------------------------------------------------------------
    [Fact]
    public async Task GetUserEntityByDomainAndEidAsync_ReturnsNull_WhenEidMissing()
    {
        var result = await _service.GetUserEntityByDomainAndEidAsync(
            "dom",
            "",
            CreateCurrentUser(1)
        );
        Assert.Null(result);
    }

    // -------------------------------------------------------------------
    // GET DTO BY DOMAIN + EID (project restricted)
    // -------------------------------------------------------------------
    [Fact]
    public async Task GetUserByDomainAndEidAsync_ReturnsNull_WhenProjectDifferent()
    {
        var user = new User { UserId = 1, ProjectId = 5 };
        _repo.Setup(r => r.GetByDomainAndEidAsync("dom", "123")).ReturnsAsync(user);

        var result = await _service.GetUserByDomainAndEidAsync("dom", "123", CreateCurrentUser(1));

        Assert.Null(result);
    }

    // -------------------------------------------------------------------
    // GET USERS BY PROJECT ID
    // -------------------------------------------------------------------
    [Fact]
    public async Task GetUsersByProjectIdAsync_ReturnsNull_WhenProjectIdInvalid()
    {
        var result = await _service.GetUsersByProjectIdAsync(0);
        Assert.Null(result);
    }

    // -------------------------------------------------------------------
    // GET USER ENTITY BY ID
    // -------------------------------------------------------------------
    [Fact]
    public async Task GetUserEntityByIdAsync_ReturnsUser()
    {
        var user = new User { UserId = 5 };
        _repo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(user);

        var result = await _service.GetUserEntityByIdAsync(5);

        Assert.Equal(5, result!.UserId);
    }
}
