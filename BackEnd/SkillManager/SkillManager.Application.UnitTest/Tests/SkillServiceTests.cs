using FluentValidation;
using Moq;
using SkillManager.Application.DTOs.Skill;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Application.Services;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.Exceptions;

namespace SkillManager.Application.UnitTest.Tests;

public class SkillServiceTests
{
    private readonly Mock<ISkillRepository> _mockSkillRepo;
    private readonly Mock<ICategoryRepository> _mockCategoryRepo;
    private readonly Mock<IValidator<UpdateSkillDto>> _mockValidator;
    private readonly SkillService _service;

    public SkillServiceTests()
    {
        _mockSkillRepo = new Mock<ISkillRepository>();
        _mockCategoryRepo = new Mock<ICategoryRepository>();
        _mockValidator = new Mock<IValidator<UpdateSkillDto>>();
        _service = new SkillService(
            _mockSkillRepo.Object,
            _mockCategoryRepo.Object,
            _mockValidator.Object
        );
    }

    [Fact]
    public async Task GetSkillByIdAsync_ReturnsSkill_WhenExists()
    {
        var skill = new Skill
        {
            SkillId = 1,
            Code = "S1",
            Label = "Skill1",
        };
        _mockSkillRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(skill);

        var result = await _service.GetSkillByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("S1", result.Code);
    }

    [Fact]
    public async Task GetSkillByIdAsync_ThrowsNotFound_WhenMissing()
    {
        _mockSkillRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Skill?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetSkillByIdAsync(1));
    }

    [Fact]
    public async Task GetSkillByCodeAsync_ReturnsSkill_WhenExists()
    {
        var skill = new Skill
        {
            SkillId = 1,
            Code = "S1",
            Label = "Skill1",
        };
        _mockSkillRepo.Setup(r => r.GetByCodeAsync("S1")).ReturnsAsync(skill);

        var result = await _service.GetSkillByCodeAsync("S1");

        Assert.NotNull(result);
        Assert.Equal("S1", result.Code);
    }

    [Fact]
    public async Task GetSkillByCodeAsync_ThrowsValidation_WhenEmptyCode()
    {
        await Assert.ThrowsAsync<ValidationException>(() => _service.GetSkillByCodeAsync(""));
    }

    [Fact]
    public async Task GetSkillByCodeAsync_ThrowsNotFound_WhenMissing()
    {
        _mockSkillRepo.Setup(r => r.GetByCodeAsync("S1")).ReturnsAsync((Skill?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetSkillByCodeAsync("S1"));
    }

    [Fact]
    public async Task GetAllSkillsAsync_ReturnsAllSkills()
    {
        var skills = new List<Skill>
        {
            new Skill
            {
                SkillId = 1,
                Code = "S1",
                Label = "Skill1",
            },
            new Skill
            {
                SkillId = 2,
                Code = "S2",
                Label = "Skill2",
            },
        };
        _mockSkillRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(skills);

        var result = await _service.GetAllSkillsAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CreateSkillAsync_Throws_WhenCodeExists()
    {
        var dto = new CreateSkillDto { Code = "S1", Label = "Skill1" };
        _mockSkillRepo.Setup(r => r.GetByCodeAsync("S1")).ReturnsAsync(new Skill());

        await Assert.ThrowsAsync<ValidationException>(() => _service.CreateSkillAsync(dto));
    }

    [Fact]
    public async Task CreateSkillAsync_ReturnsTrue_WhenSuccess()
    {
        var dto = new CreateSkillDto { Code = "S1", Label = "Skill1" };
        _mockSkillRepo.Setup(r => r.GetByCodeAsync("S1")).ReturnsAsync((Skill?)null);
        _mockSkillRepo.Setup(r => r.AddAsync(It.IsAny<Skill>())).ReturnsAsync(true);

        var result = await _service.CreateSkillAsync(dto);

        Assert.True(result);
        _mockSkillRepo.Verify(r => r.AddAsync(It.IsAny<Skill>()), Times.Once);
    }

    [Fact]
    public async Task UpdateSkillAsync_ThrowsNotFound_WhenSkillMissing()
    {
        var dto = new UpdateSkillDto { Label = "Updated" };
        _mockSkillRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Skill?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateSkillAsync(1, dto));
    }

    [Fact]
    public async Task UpdateSkillAsync_ReturnsSkillDto_WhenNoChanges()
    {
        var skill = new Skill
        {
            SkillId = 1,
            Label = "S1",
            Code = "C1",
        };
        _mockSkillRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(skill);

        var dto = new UpdateSkillDto(); // no changes
        var result = await _service.UpdateSkillAsync(1, dto);

        Assert.Equal("S1", result.Label);
    }

    [Fact]
    public async Task UpdateSkillAsync_ReturnsSkillDto_WhenUpdated()
    {
        var skill = new Skill
        {
            SkillId = 1,
            Label = "S1",
            Code = "C1",
            CategoryId = 1,
            SubCategoryId = 1,
        };
        var dto = new UpdateSkillDto
        {
            Label = "Updated",
            Code = "C2",
            CategoryId = 2,
            SubCategoryId = 2,
        };

        _mockSkillRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(skill);
        _mockSkillRepo.Setup(r => r.GetByCodeAsync("C2")).ReturnsAsync((Skill?)null);
        _mockCategoryRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(new Category());
        _mockCategoryRepo.Setup(r => r.GetSubCategoryByIdAsync(2)).ReturnsAsync(new SubCategory());
        _mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<UpdateSkillDto>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _mockSkillRepo.Setup(r => r.UpdateAsync(It.IsAny<Skill>())).ReturnsAsync(true);
        _mockSkillRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(skill);

        var result = await _service.UpdateSkillAsync(1, dto);

        Assert.Equal("Updated", result.Label);
        Assert.Equal("C2", result.Code);
        Assert.Equal(2, result.CategoryId);
        Assert.Equal(2, result.SubCategoryId);
    }

    [Fact]
    public async Task DeleteSkillAsync_ThrowsNotFound_WhenMissing()
    {
        _mockSkillRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Skill?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteSkillAsync(1));
    }

    [Fact]
    public async Task DeleteSkillAsync_ReturnsTrue_WhenDeleted()
    {
        var skill = new Skill { SkillId = 1 };
        _mockSkillRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(skill);
        _mockSkillRepo.Setup(r => r.DeleteAsync(skill)).ReturnsAsync(true);

        var result = await _service.DeleteSkillAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task GetSkillsByCategoryAsync_Throws_WhenCategoryMissing()
    {
        _mockCategoryRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Category?)null);

        await Assert.ThrowsAsync<ValidationException>(() => _service.GetSkillsByCategoryAsync(1));
    }

    [Fact]
    public async Task GetSkillsByCategoryAsync_ReturnsSkills()
    {
        var category = new Category { CategoryId = 1 };
        var skills = new List<Skill> { new Skill { SkillId = 1 } };
        _mockCategoryRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
        _mockSkillRepo.Setup(r => r.GetByCategoryAsync(category)).ReturnsAsync(skills);

        var result = await _service.GetSkillsByCategoryAsync(1);

        Assert.Single(result);
    }

    [Fact]
    public async Task GetSkillsBySubCategoryAsync_Throws_WhenSubCategoryMissing()
    {
        _mockCategoryRepo.Setup(r => r.GetSubCategoryByIdAsync(1)).ReturnsAsync((SubCategory?)null);

        await Assert.ThrowsAsync<ValidationException>(() =>
            _service.GetSkillsBySubCategoryAsync(1)
        );
    }

    [Fact]
    public async Task GetSkillsBySubCategoryAsync_ReturnsSkills()
    {
        var subCategory = new SubCategory { SubCategoryId = 1 };
        var skills = new List<Skill> { new Skill { SkillId = 1 } };
        _mockCategoryRepo.Setup(r => r.GetSubCategoryByIdAsync(1)).ReturnsAsync(subCategory);
        _mockSkillRepo.Setup(r => r.GetBySubCategoryAsync(subCategory)).ReturnsAsync(skills);

        var result = await _service.GetSkillsBySubCategoryAsync(1);

        Assert.Single(result);
    }

    [Fact]
    public async Task GetCriticalSkillsAsync_ReturnsSkills()
    {
        var skills = new List<Skill> { new Skill { SkillId = 1 } };
        _mockSkillRepo.Setup(r => r.GetCriticalSkillsAsync()).ReturnsAsync(skills);

        var result = await _service.GetCriticalSkillsAsync();

        Assert.Single(result);
    }

    [Fact]
    public async Task GetProjectRequiredSkillsAsync_ReturnsSkills()
    {
        var skills = new List<Skill> { new Skill { SkillId = 1 } };
        _mockSkillRepo.Setup(r => r.GetProjectRequiredSkillsAsync()).ReturnsAsync(skills);

        var result = await _service.GetProjectRequiredSkillsAsync();

        Assert.Single(result);
    }

    [Fact]
    public async Task GetSkillsByRequiredLevelAsync_Throws_WhenLevelInvalid()
    {
        await Assert.ThrowsAsync<ValidationException>(() =>
            _service.GetSkillsByRequiredLevelAsync(0)
        );
        await Assert.ThrowsAsync<ValidationException>(() =>
            _service.GetSkillsByRequiredLevelAsync(5)
        );
    }

    [Fact]
    public async Task GetSkillsByRequiredLevelAsync_ReturnsSkills()
    {
        var skills = new List<Skill>
        {
            new Skill { SkillId = 1, RequiredLevel = 2 },
            new Skill { SkillId = 2, RequiredLevel = 3 },
        };
        _mockSkillRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(skills);

        var result = await _service.GetSkillsByRequiredLevelAsync(2);

        Assert.Single(result);
        Assert.Equal(2, result.First().RequiredLevel);
    }

    [Fact]
    public async Task SearchSkillsAsync_Throws_WhenTermInvalid()
    {
        await Assert.ThrowsAsync<ValidationException>(() => _service.SearchSkillsAsync(null!));
        await Assert.ThrowsAsync<ValidationException>(() => _service.SearchSkillsAsync("a"));
    }

    [Fact]
    public async Task SearchSkillsAsync_ReturnsMatchingSkills()
    {
        var skills = new List<Skill>
        {
            new Skill
            {
                SkillId = 1,
                Code = "C1",
                Label = "Skill1",
            },
            new Skill
            {
                SkillId = 2,
                Code = "C2",
                Label = "Skill2",
            },
        };
        _mockSkillRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(skills);

        var result = await _service.SearchSkillsAsync("C1");

        Assert.Single(result);
        Assert.Equal(1, result.First().SkillId);
    }
}
