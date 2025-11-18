using Moq;
using SkillManager.Application.DTOs.Category;
using SkillManager.Application.DTOs.Skill;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Application.Services;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.DTOs.Skill;

namespace SkillManager.Application.UnitTest.Tests;

public class UserSkillServiceTests
{
    private readonly Mock<IUserSkillRepository> _mockUserSkillRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<ICategoryRepository> _mockCategoryRepo;
    private readonly UserSkillService _service;

    public UserSkillServiceTests()
    {
        _mockUserSkillRepo = new Mock<IUserSkillRepository>();
        _mockUserRepo = new Mock<IUserRepository>();
        _mockCategoryRepo = new Mock<ICategoryRepository>();

        _service = new UserSkillService(
            _mockUserSkillRepo.Object,
            _mockUserRepo.Object,
            _mockCategoryRepo.Object
        );
    }

    private static User CreateUser(int userId = 1, int projectId = 1, int? teamId = null)
    {
        return new User
        {
            UserId = userId,
            ProjectId = projectId,
            TeamId = teamId,
        };
    }

    private static UserSkill CreateUserSkill(
        int userId = 1,
        int skillId = 10,
        int levelId = 2,
        DateTime? updated = null,
        string skillName = "SkillA",
        string skillCode = "SKA",
        int categoryId = 100,
        string categoryName = "CatA",
        string levelName = "Intermediate",
        int levelPoints = 20,
        int requiredLevel = 5
    )
    {
        updated ??= new DateTime(2023, 1, 1);

        var category = new Category
        {
            CategoryId = categoryId,
            Name = categoryName,
            CategoryTypeId = 1,
            CategoryType = new CategoryType { CategoryTypeId = 1, Name = "Type1" },
        };

        var skill = new Skill
        {
            SkillId = skillId,
            Label = skillName,
            Code = skillCode,
            CategoryId = categoryId,
            Category = category,
            RequiredLevel = requiredLevel,
            ProjectRequiresSkill = false,
        };

        var level = new Level
        {
            LevelId = levelId,
            Name = levelName,
            Points = levelPoints,
        };

        return new UserSkill
        {
            UserId = userId,
            SkillId = skillId,
            LevelId = levelId,
            UpdatedTime = updated.Value,
            Skill = skill,
            Level = level,
            User = new User
            {
                UserId = userId,
                FirstName = "F",
                LastName = "L",
            },
        };
    }

    // -----------------------
    // GetMySkillsAsync
    // -----------------------
    [Fact]
    public async Task GetMySkillsAsync_Throws_When_UserNotFound()
    {
        _mockUserRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.GetMySkillsAsync(5));
    }

    [Fact]
    public async Task GetMySkillsAsync_ReturnsMappedViewModels()
    {
        var us = CreateUserSkill(userId: 2);
        _mockUserRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(CreateUser(2));
        _mockUserSkillRepo.Setup(r => r.GetUserSkillsAsync(2)).ReturnsAsync(new[] { us });

        var result = (await _service.GetMySkillsAsync(2)).ToList();

        Assert.Single(result);
        Assert.Equal(us.Skill.Label, result[0].SkillLabel);
    }

    // -----------------------
    // GetUserSkillsByCategoryAsync
    // -----------------------
    [Fact]
    public async Task GetUserSkillsByCategoryAsync_Throws_When_UserNotFound()
    {
        _mockUserRepo.Setup(r => r.GetByIdAsync(9)).ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.GetUserSkillsByCategoryAsync(1, 9)
        );
    }

    [Fact]
    public async Task GetUserSkillsByCategoryAsync_Throws_When_CategoryNotFound()
    {
        _mockUserRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(CreateUser(2));
        _mockCategoryRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Category?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.GetUserSkillsByCategoryAsync(99, 2)
        );
    }

    [Fact]
    public async Task GetUserSkillsByCategoryAsync_Throws_When_RepoReturnsNull()
    {
        _mockUserRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(CreateUser(2));
        var category = new Category { CategoryId = 3, Name = "C" };
        _mockCategoryRepo.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(category);
        _mockUserSkillRepo
            .Setup(r => r.GetSkillsByCategory(category, It.IsAny<User>()))
            .ReturnsAsync((IEnumerable<UserSkill>?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.GetUserSkillsByCategoryAsync(3, 2)
        );
    }

    [Fact]
    public async Task GetUserSkillsByCategoryAsync_ReturnsMappedWhenOk()
    {
        var user = CreateUser(2);
        _mockUserRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(user);
        var category = new Category { CategoryId = 7, Name = "C7" };
        var us = CreateUserSkill(userId: 2, categoryId: 7);
        _mockCategoryRepo.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(category);
        _mockUserSkillRepo
            .Setup(r => r.GetSkillsByCategory(category, user))
            .ReturnsAsync(new[] { us });

        var result = (await _service.GetUserSkillsByCategoryAsync(7, 2)).ToList();

        Assert.Single(result);
        Assert.Equal(us.Skill.Label, result[0].Label);
    }

    // -----------------------
    // Skill gaps
    // -----------------------
    [Fact]
    public async Task GetSkillGapsAsync_Returns_ListFromRepo()
    {
        var gaps = new List<SkillGapDto>
        {
            new SkillGapDto
            {
                SkillId = 1,
                SkillCode = "SK1",
                SkillName = "Skill1",
                UserLevel = 3,
                RequiredLevel = 5,
            },
        };
        _mockUserSkillRepo.Setup(r => r.GetSkillGapsAsync(1)).ReturnsAsync(gaps);

        var result = await _service.GetSkillGapsAsync(1);

        Assert.Equal(gaps, result);
        Assert.Equal(2, result[0].GapSize); // computed property works
    }

    [Fact]
    public async Task GetSkillGapsByCategoryAsync_Returns_ListFromRepo()
    {
        var gaps = new List<CategoryGapDto> { new CategoryGapDto { CategoryId = 10 } };
        _mockUserSkillRepo.Setup(r => r.GetSkillGapsByCategoryAsync(1)).ReturnsAsync(gaps);

        var result = await _service.GetSkillGapsByCategoryAsync(1);

        Assert.Equal(gaps, result);
    }

    // -----------------------
    // GetUserSkillsLevels
    // -----------------------
    [Fact]
    public async Task GetUserSkillsLevels_ReturnsMappedViewModels()
    {
        var us = CreateUserSkill();
        _mockUserSkillRepo.Setup(r => r.GetAllUserSkillsLevels()).ReturnsAsync(new[] { us });

        var result = (await _service.GetUserSkillsLevels()).ToList();

        Assert.Single(result);
        Assert.Equal(us.Skill.Label, result[0].SkillLabel);
    }

    // -----------------------
    // AddSkillAsync
    // -----------------------
    [Fact]
    public async Task AddSkillAsync_Throws_When_UserNotFound()
    {
        _mockUserRepo.Setup(r => r.GetByIdAsync(50)).ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.AddSkillAsync(
                50,
                new AddUserSkillDto
                {
                    SkillId = 1,
                    LevelId = 1,
                    UpdatedTime = DateTime.UtcNow,
                }
            )
        );
    }

    [Fact]
    public async Task AddSkillAsync_Throws_When_ExistingFound()
    {
        var user = CreateUser(3);
        _mockUserRepo.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(user);
        _mockUserSkillRepo.Setup(r => r.GetByCompositeKeyAsync(3, 5)).ReturnsAsync(new UserSkill());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.AddSkillAsync(
                3,
                new AddUserSkillDto
                {
                    SkillId = 5,
                    LevelId = 1,
                    UpdatedTime = DateTime.UtcNow,
                }
            )
        );
    }

    [Fact]
    public async Task AddSkillAsync_AddsAndSaves_When_New()
    {
        var user = CreateUser(4);
        _mockUserRepo.Setup(r => r.GetByIdAsync(4)).ReturnsAsync(user);
        _mockUserSkillRepo
            .Setup(r => r.GetByCompositeKeyAsync(4, 8))
            .ReturnsAsync((UserSkill?)null);

        var dto = new AddUserSkillDto
        {
            SkillId = 8,
            LevelId = 2,
            UpdatedTime = new DateTime(2024, 1, 1),
        };

        await _service.AddSkillAsync(4, dto);

        _mockUserSkillRepo.Verify(
            r =>
                r.AddAsync(
                    It.Is<UserSkill>(us => us.UserId == 4 && us.SkillId == 8 && us.LevelId == 2)
                ),
            Times.Once
        );
        _mockUserSkillRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    // -----------------------
    // UpdateSkillAsync
    // -----------------------
    [Fact]
    public async Task UpdateSkillAsync_Throws_When_NotFound()
    {
        _mockUserSkillRepo
            .Setup(r => r.GetByCompositeKeyAsync(9, 20))
            .ReturnsAsync((UserSkill?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UpdateSkillAsync(
                9,
                new UpdateUserSkillsDto
                {
                    SkillId = 20,
                    LevelId = 3,
                    UpdatedTime = DateTime.UtcNow,
                }
            )
        );
    }

    [Fact]
    public async Task UpdateSkillAsync_UpdatesAndSaves_When_Found()
    {
        var us = CreateUserSkill(
            userId: 6,
            skillId: 11,
            levelId: 1,
            updated: new DateTime(2021, 1, 1)
        );
        _mockUserSkillRepo.Setup(r => r.GetByCompositeKeyAsync(6, 11)).ReturnsAsync(us);

        var dto = new UpdateUserSkillsDto
        {
            SkillId = 11,
            LevelId = 4,
            UpdatedTime = new DateTime(2025, 5, 5),
        };

        var result = await _service.UpdateSkillAsync(6, dto);

        Assert.True(result);
        Assert.Equal(4, us.LevelId);
        Assert.Equal(dto.UpdatedTime, us.UpdatedTime);
        _mockUserSkillRepo.Verify(r => r.UpdateAsync(us), Times.Once);
        _mockUserSkillRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    // -----------------------
    // GetAllUserSkillsAsync
    // -----------------------
    [Fact]
    public async Task GetAllUserSkillsAsync_MapsAll()
    {
        var us = CreateUserSkill();
        _mockUserSkillRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new[] { us });

        var result = (await _service.GetAllUserSkillsAsync()).ToList();

        Assert.Single(result);
        Assert.Equal(us.Skill.Label, result[0].SkillName);
    }

    // -----------------------
    // GetAllUserSkillsByTeamAsync
    // -----------------------
    [Fact]
    public async Task GetAllUserSkillsByTeamAsync_Throws_When_UserMissing()
    {
        _mockUserRepo.Setup(r => r.GetByIdAsync(77)).ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.GetAllUserSkillsByTeamAsync(77)
        );
    }

    [Fact]
    public async Task GetAllUserSkillsByTeamAsync_ReturnsMapped_When_Found()
    {
        var user = CreateUser(8, projectId: 1, teamId: 99);
        _mockUserRepo.Setup(r => r.GetByIdAsync(8)).ReturnsAsync(user);
        var us = CreateUserSkill(userId: 8);
        _mockUserSkillRepo.Setup(r => r.GetAllByTeamAsync(user)).ReturnsAsync(new[] { us });

        var result = (await _service.GetAllUserSkillsByTeamAsync(8)).ToList();

        Assert.Single(result);
        Assert.Equal(us.Skill.Label, result[0].SkillName);
    }

    // -----------------------
    // FilterBySkillAsync
    // -----------------------
    [Fact]
    public async Task FilterBySkillAsync_ReturnsMapped()
    {
        var us = CreateUserSkill();
        _mockUserSkillRepo.Setup(r => r.FilterBySkillAsync("SK")).ReturnsAsync(new[] { us });

        var result = (await _service.FilterBySkillAsync("SK")).ToList();

        Assert.Single(result);
        Assert.Equal(us.Skill.Label, result[0].SkillName);
    }

    // -----------------------
    // DeleteUserSkillAsync
    // -----------------------
    [Fact]
    public async Task DeleteUserSkillAsync_Throws_When_NoSkills()
    {
        _mockUserSkillRepo
            .Setup(r => r.GetUserSkillsAsync(100))
            .ReturnsAsync(Enumerable.Empty<UserSkill>());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.DeleteUserSkillAsync(100)
        );
    }

    [Fact]
    public async Task DeleteUserSkillAsync_DeletesAllAndSaves()
    {
        var us1 = CreateUserSkill(userId: 11, skillId: 1);
        var us2 = CreateUserSkill(userId: 11, skillId: 2);
        _mockUserSkillRepo.Setup(r => r.GetUserSkillsAsync(11)).ReturnsAsync(new[] { us1, us2 });

        await _service.DeleteUserSkillAsync(11);

        _mockUserSkillRepo.Verify(r => r.DeleteAsync(11, 1), Times.Once);
        _mockUserSkillRepo.Verify(r => r.DeleteAsync(11, 2), Times.Once);
        _mockUserSkillRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    // -----------------------
    // GetAllCategories
    // -----------------------
    [Fact]
    public async Task GetAllCategories_ReturnsMappedCategoryDtos()
    {
        var cat = new Category
        {
            CategoryId = 1,
            Name = "Cat1",
            CategoryTypeId = 2,
            CategoryType = new CategoryType { CategoryTypeId = 2, Name = "T2" },
        };
        _mockCategoryRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new[] { cat });

        var result = await _service.GetAllCategories();

        Assert.Single(result);
        Assert.Equal(cat.Name, result[0].Name);
    }

    // -----------------------
    // GetCategoryNavigationAsync - when user skills retrieval throws -> empty list
    // -----------------------
    [Fact]
    public async Task GetCategoryNavigationAsync_ReturnsEmptyUserSkills_When_GetUserSkillsByCategoryThrows()
    {
        _mockCategoryRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new Category[] { });
        // Make GetUserSkillsByCategoryAsync throw by mocking underlying repos:
        _mockUserRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(CreateUser(5));
        _mockCategoryRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Category?)null); // cause throw inside GetUserSkillsByCategoryAsync

        var vm = await _service.GetCategoryNavigationAsync(1, 5);

        Assert.NotNull(vm);
        Assert.Empty(vm.UserSkills);
        Assert.NotNull(vm.CategoryTypes);
    }

    [Fact]
    public async Task GetCategoryNavigationAsync_ReturnsUserSkills_When_Valid()
    {
        // Prepare categories for CategoryTypes
        var cat1 = new Category
        {
            CategoryId = 1,
            Name = "C1",
            CategoryTypeId = 1,
            CategoryType = new CategoryType { CategoryTypeId = 1, Name = "A" },
        };
        var cat2 = new Category
        {
            CategoryId = 2,
            Name = "C2",
            CategoryTypeId = 1,
            CategoryType = new CategoryType { CategoryTypeId = 1, Name = "A" },
        };
        _mockCategoryRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new[] { cat1, cat2 });

        // Prepare user & category skills
        var user = CreateUser(12);
        var cat = new Category { CategoryId = 1, Name = "C1" };
        _mockUserRepo.Setup(r => r.GetByIdAsync(12)).ReturnsAsync(user);
        _mockCategoryRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(cat);

        var us = CreateUserSkill(userId: 12, categoryId: 1);
        _mockUserSkillRepo.Setup(r => r.GetSkillsByCategory(cat, user)).ReturnsAsync(new[] { us });

        var vm = await _service.GetCategoryNavigationAsync(1, 12);

        Assert.NotNull(vm);
        Assert.Single(vm.UserSkills);
        Assert.Equal(1, vm.SelectedCategoryId);
        Assert.NotEmpty(vm.CategoryTypes);
    }

    // -----------------------
    // GetCategoryTypesWithCategoriesAsync
    // -----------------------
    [Fact]
    public async Task GetCategoryTypesWithCategoriesAsync_GroupsAndOrdersTypes()
    {
        var ct1 = new CategoryType { CategoryTypeId = 1, Name = "B" };
        var ct2 = new CategoryType { CategoryTypeId = 2, Name = "A" };
        var c1 = new Category
        {
            CategoryId = 1,
            Name = "C1",
            CategoryTypeId = 1,
            CategoryType = ct1,
        };
        var c2 = new Category
        {
            CategoryId = 2,
            Name = "C2",
            CategoryTypeId = 2,
            CategoryType = ct2,
        };
        _mockCategoryRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new[] { c1, c2 });

        var result = await _service.GetCategoryTypesWithCategoriesAsync();

        // Ordered by Name => "A" then "B"
        Assert.Equal(2, result.Count);
        Assert.Equal("A", result[0].Name);
        Assert.Equal("B", result[1].Name);
        // categories mapped inside
        Assert.Contains(result[0].Categories, x => x.CategoryId == 2);
        Assert.Contains(result[1].Categories, x => x.CategoryId == 1);
    }

    // -----------------------
    // GetLastUpdatedTimeAsync
    // -----------------------
    [Fact]
    public async Task GetLastUpdatedTimeAsync_ReturnsNull_When_NoSkills()
    {
        _mockUserSkillRepo
            .Setup(r => r.GetUserSkillsAsync(33))
            .ReturnsAsync(Enumerable.Empty<UserSkill>());

        var result = await _service.GetLastUpdatedTimeAsync(33);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetLastUpdatedTimeAsync_ReturnsMaxUpdatedTime()
    {
        var us1 = CreateUserSkill(userId: 44, updated: new DateTime(2020, 1, 1));
        var us2 = CreateUserSkill(userId: 44, skillId: 2, updated: new DateTime(2023, 3, 3));
        _mockUserSkillRepo.Setup(r => r.GetUserSkillsAsync(44)).ReturnsAsync(new[] { us1, us2 });

        var result = await _service.GetLastUpdatedTimeAsync(44);

        Assert.Equal(new DateTime(2023, 3, 3), result);
    }

    // -----------------------
    // GetLastUpdatedTimeForSkillAsync
    // -----------------------
    [Fact]
    public async Task GetLastUpdatedTimeForSkillAsync_ReturnsNull_When_NotFound()
    {
        _mockUserSkillRepo
            .Setup(r => r.GetByCompositeKeyAsync(10, 99))
            .ReturnsAsync((UserSkill?)null);

        var result = await _service.GetLastUpdatedTimeForSkillAsync(10, 99);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetLastUpdatedTimeForSkillAsync_ReturnsUpdatedTime()
    {
        var dt = new DateTime(2022, 2, 2);
        var us = CreateUserSkill(userId: 10, skillId: 99, updated: dt);
        _mockUserSkillRepo.Setup(r => r.GetByCompositeKeyAsync(10, 99)).ReturnsAsync(us);

        var result = await _service.GetLastUpdatedTimeForSkillAsync(10, 99);

        Assert.Equal(dt, result);
    }

    // -----------------------
    // GetLastUpdatedTimesByCategoryAsync
    // -----------------------
    [Fact]
    public async Task GetLastUpdatedTimesByCategoryAsync_ReturnsEmpty_When_CategoryMissing()
    {
        _mockCategoryRepo.Setup(r => r.GetByIdAsync(500)).ReturnsAsync((Category?)null);

        var result = await _service.GetLastUpdatedTimesByCategoryAsync(1, 500);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetLastUpdatedTimesByCategoryAsync_ReturnsDictionary_When_Found()
    {
        var user = CreateUser(55);
        var cat = new Category { CategoryId = 77, Name = "C77" };
        var us1 = CreateUserSkill(
            userId: 55,
            skillId: 3,
            updated: new DateTime(2021, 1, 1),
            categoryId: 77
        );
        var us2 = CreateUserSkill(
            userId: 55,
            skillId: 4,
            updated: new DateTime(2022, 2, 2),
            categoryId: 77
        );

        _mockCategoryRepo.Setup(r => r.GetByIdAsync(77)).ReturnsAsync(cat);
        _mockUserRepo.Setup(r => r.GetByIdAsync(55)).ReturnsAsync(user);
        _mockUserSkillRepo
            .Setup(r => r.GetSkillsByCategory(cat, user))
            .ReturnsAsync(new[] { us1, us2 });

        var result = await _service.GetLastUpdatedTimesByCategoryAsync(55, 77);

        Assert.Equal(2, result.Count);
        Assert.Equal(new DateTime(2021, 1, 1), result[3]);
        Assert.Equal(new DateTime(2022, 2, 2), result[4]);
    }

    // -----------------------
    // GetUserSkillsByUserIdAsync
    // -----------------------
    [Fact]
    public async Task GetUserSkillsByUserIdAsync_MapsToDto()
    {
        var us = CreateUserSkill(userId: 88);
        _mockUserSkillRepo.Setup(r => r.GetUserSkillsAsync(88)).ReturnsAsync(new[] { us });

        var result = (await _service.GetUserSkillsByUserIdAsync(88)).ToList();

        Assert.Single(result);
        Assert.Equal(us.Skill.Label, result[0].SkillName);
    }
}
