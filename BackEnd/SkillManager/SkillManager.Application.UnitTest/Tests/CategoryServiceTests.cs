using FluentValidation;
using Moq;
using SkillManager.Application.DTOs.Category;
using SkillManager.Application.DTOs.SubCategory;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Application.Services;
using SkillManager.Application.Validators.Category;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.Exceptions;

namespace SkillManager.Application.UnitTest.Tests;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _mockRepo;
    private readonly Mock<IValidator<CreateCategoryDto>> _mockCreateCategoryValidator;
    private readonly Mock<IValidator<UpdateCategoryDto>> _mockUpdateCategoryValidator;
    private readonly Mock<IValidator<CreateSubCategoryDto>> _mockCreateSubValidator;
    private readonly Mock<IValidator<UpdateSubCategoryDto>> _mockUpdateSubValidator;
    private readonly Mock<IValidator<CreateCategoryTypeDto>> _mockCreateTypeValidator;
    private readonly Mock<IValidator<UpdateCategoryTypeDto>> _mockUpdateTypeValidator;
    private readonly CategoryService _service;

    public CategoryServiceTests()
    {
        _mockRepo = new Mock<ICategoryRepository>();
        _mockCreateCategoryValidator = new Mock<IValidator<CreateCategoryDto>>();
        _mockUpdateCategoryValidator = new Mock<IValidator<UpdateCategoryDto>>();
        _mockCreateSubValidator = new Mock<IValidator<CreateSubCategoryDto>>();
        _mockUpdateSubValidator = new Mock<IValidator<UpdateSubCategoryDto>>();
        _mockCreateTypeValidator = new Mock<IValidator<CreateCategoryTypeDto>>();
        _mockUpdateTypeValidator = new Mock<IValidator<UpdateCategoryTypeDto>>();

        _service = new CategoryService(
            _mockRepo.Object,
            _mockCreateCategoryValidator.Object,
            _mockUpdateCategoryValidator.Object,
            _mockCreateSubValidator.Object,
            _mockUpdateSubValidator.Object,
            _mockCreateTypeValidator.Object,
            _mockUpdateTypeValidator.Object
        );
    }

    // -----------------------
    // Category Methods
    // -----------------------
    [Fact]
    public async Task GetCategoryByIdAsync_Throws_WhenInvalidId()
    {
        await Assert.ThrowsAsync<ValidationException>(() => _service.GetCategoryByIdAsync(0));
    }

    [Fact]
    public async Task GetCategoryByIdAsync_Throws_WhenNotFound()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Category?)null);
        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetCategoryByIdAsync(1));
    }

    [Fact]
    public async Task GetCategoryByIdAsync_ReturnsDto_WhenFound()
    {
        var cat = new Category { CategoryId = 1, Name = "Cat1" };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(cat);

        var result = await _service.GetCategoryByIdAsync(1);
        Assert.Equal("Cat1", result.Name);
    }

    [Fact]
    public async Task GetCategoryByNameAsync_Throws_WhenEmpty()
    {
        await Assert.ThrowsAsync<ValidationException>(() => _service.GetCategoryByNameAsync(""));
    }

    [Fact]
    public async Task GetCategoryByNameAsync_Throws_WhenNotFound()
    {
        _mockRepo.Setup(r => r.GetByNameAsync("Cat1")).ReturnsAsync((Category?)null);
        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetCategoryByNameAsync("Cat1"));
    }

    [Fact]
    public async Task GetCategoryByNameAsync_ReturnsDto_WhenFound()
    {
        var cat = new Category { CategoryId = 1, Name = "Cat1" };
        _mockRepo.Setup(r => r.GetByNameAsync("Cat1")).ReturnsAsync(cat);

        var result = await _service.GetCategoryByNameAsync("Cat1");
        Assert.Equal(1, result.CategoryId);
    }

    [Fact]
    public async Task GetAllCategoriesAsync_ReturnsDtos()
    {
        var list = new List<Category>
        {
            new Category { CategoryId = 1, Name = "A" },
        };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

        var result = await _service.GetAllCategoriesAsync();
        Assert.Single(result);
    }

    [Fact]
    public async Task GetAllSubCategoriesAsync_ReturnsDtos()
    {
        var list = new List<SubCategory>
        {
            new SubCategory { SubCategoryId = 1, Name = "Sub1" },
        };
        _mockRepo.Setup(r => r.GetAllSubCategoriesAsync()).ReturnsAsync(list);

        var result = await _service.GetAllSubCategoriesAsync();
        Assert.Single(result);
    }

    [Fact]
    public async Task GetCategoriesByTypeAsync_Throws_WhenInvalidId()
    {
        await Assert.ThrowsAsync<ValidationException>(() => _service.GetCategoriesByTypeAsync(0));
    }

    [Fact]
    public async Task GetCategoriesByTypeAsync_Throws_WhenTypeNotFound()
    {
        _mockRepo.Setup(r => r.GetCategoryTypeByIdAsync(1)).ReturnsAsync((CategoryType?)null);
        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetCategoriesByTypeAsync(1));
    }

    [Fact]
    public async Task GetCategoriesByTypeAsync_ReturnsDtos()
    {
        var type = new CategoryType { CategoryTypeId = 1 };
        _mockRepo.Setup(r => r.GetCategoryTypeByIdAsync(1)).ReturnsAsync(type);
        _mockRepo
            .Setup(r => r.GetByCategoryTypeAsync(type))
            .ReturnsAsync(
                new List<Category>
                {
                    new Category { CategoryId = 1, Name = "C1" },
                }
            );

        var result = await _service.GetCategoriesByTypeAsync(1);
        Assert.Single(result);
    }

    [Fact]
    public async Task CreateCategoryAsync_Throws_WhenValidatorFails()
    {
        var dto = new CreateCategoryDto { Name = "X", CategoryTypeId = 1 };
        _mockCreateCategoryValidator
            .Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(
                new FluentValidation.Results.ValidationResult(
                    new[] { new FluentValidation.Results.ValidationFailure("Name", "Error") }
                )
            );

        await Assert.ThrowsAsync<ValidationException>(() => _service.CreateCategoryAsync(dto));
    }

    [Fact]
    public async Task CreateCategoryAsync_Throws_WhenNameExists()
    {
        var dto = new CreateCategoryDto { Name = "X", CategoryTypeId = 1 };

        // Make sure validator is used
        var validator = new CreateCategoryValidator();

        var service = new CategoryService(_mockRepo.Object, validator);

        _mockRepo
            .Setup(r => r.GetByNameAsync(It.Is<string>(s => s == "X")))
            .ReturnsAsync(new Category());

        await Assert.ThrowsAsync<ValidationException>(() => service.CreateCategoryAsync(dto));
    }

    [Fact]
    public async Task CreateCategoryAsync_Throws_WhenCategoryTypeMissing()
    {
        var dto = new CreateCategoryDto { Name = "X", CategoryTypeId = 1 };

        var validator = new CreateCategoryValidator();
        var service = new CategoryService(_mockRepo.Object, validator);

        _mockRepo
            .Setup(r => r.GetByNameAsync(It.Is<string>(s => s == "X")))
            .ReturnsAsync((Category?)null);

        _mockRepo
            .Setup(r => r.GetCategoryTypeByIdAsync(It.Is<int>(i => i == 1)))
            .ReturnsAsync((CategoryType?)null);

        await Assert.ThrowsAsync<ValidationException>(() => service.CreateCategoryAsync(dto));
    }

    [Fact]
    public async Task CreateCategoryAsync_ReturnsDto_WhenSuccess()
    {
        var dto = new CreateCategoryDto { Name = "X", CategoryTypeId = 1 };
        var validator = new CreateCategoryValidator();
        var service = new CategoryService(_mockRepo.Object, validator);

        _mockRepo
            .Setup(r => r.GetCategoryTypeByIdAsync(It.Is<int>(i => i == 1)))
            .ReturnsAsync(new CategoryType { CategoryTypeId = 1 });

        _mockRepo
            .SetupSequence(r => r.GetByNameAsync(It.Is<string>(s => s == "X")))
            .ReturnsAsync((Category?)null) // first call: check existing
            .ReturnsAsync(new Category { CategoryId = 1, Name = "X" }); // second call: return created

        _mockRepo.Setup(r => r.AddAsync(It.IsAny<Category>())).ReturnsAsync(true);

        var result = await service.CreateCategoryAsync(dto);

        Assert.Equal("X", result.Name);
        Assert.Equal(1, result.CategoryId);
    }

    [Fact]
    public async Task UpdateCategoryAsync_Throws_WhenInvalidId()
    {
        await Assert.ThrowsAsync<ValidationException>(() =>
            _service.UpdateCategoryAsync(0, new UpdateCategoryDto())
        );
    }

    [Fact]
    public async Task UpdateCategoryAsync_Throws_WhenNotFound()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Category?)null);
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.UpdateCategoryAsync(1, new UpdateCategoryDto())
        );
    }

    // Additional positive/negative update tests omitted for brevity in this snippet
    // (Would include name exists, type not found, successful update)

    [Fact]
    public async Task DeleteCategoryAsync_Throws_WhenInvalidId()
    {
        await Assert.ThrowsAsync<ValidationException>(() => _service.DeleteCategoryAsync(0));
    }

    [Fact]
    public async Task DeleteCategoryAsync_Throws_WhenCategoryNotFound()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Category?)null);
        await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteCategoryAsync(1));
    }

    [Fact]
    public async Task DeleteCategoryAsync_Throws_WhenHasSkills()
    {
        _mockRepo
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(
                new Category
                {
                    CategoryId = 1,
                    Skills = new List<Skill> { new Skill() },
                }
            );
        await Assert.ThrowsAsync<ValidationException>(() => _service.DeleteCategoryAsync(1));
    }

    [Fact]
    public async Task DeleteCategoryAsync_ReturnsTrue_WhenNoSkills()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Category { CategoryId = 1 });
        _mockRepo.Setup(r => r.DeleteAsync(It.IsAny<Category>())).ReturnsAsync(true);

        var result = await _service.DeleteCategoryAsync(1);
        Assert.True(result);
    }

    // -----------------------
    // CategoryType Methods
    // -----------------------
    [Fact]
    public async Task GetCategoryTypeByIdAsync_Throws_WhenInvalid()
    {
        await Assert.ThrowsAsync<ValidationException>(() => _service.GetCategoryTypeByIdAsync(0));
    }

    [Fact]
    public async Task GetCategoryTypeByIdAsync_Throws_WhenNotFound()
    {
        _mockRepo.Setup(r => r.GetCategoryTypeByIdAsync(1)).ReturnsAsync((CategoryType?)null);
        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetCategoryTypeByIdAsync(1));
    }

    [Fact]
    public async Task GetCategoryTypeByIdAsync_ReturnsDto_WhenFound()
    {
        var type = new CategoryType { CategoryTypeId = 1, Name = "T" };
        _mockRepo.Setup(r => r.GetCategoryTypeByIdAsync(1)).ReturnsAsync(type);

        var result = await _service.GetCategoryTypeByIdAsync(1);
        Assert.Equal("T", result.Name);
    }

    [Fact]
    public async Task GetCategoryTypeByNameAsync_Throws_WhenEmpty()
    {
        await Assert.ThrowsAsync<ValidationException>(() =>
            _service.GetCategoryTypeByNameAsync("")
        );
    }

    [Fact]
    public async Task GetCategoryTypeByNameAsync_Throws_WhenNotFound()
    {
        _mockRepo.Setup(r => r.GetCategoryTypeByNameAsync("T")).ReturnsAsync((CategoryType?)null);
        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetCategoryTypeByNameAsync("T"));
    }

    [Fact]
    public async Task GetCategoryTypeByNameAsync_ReturnsDto_WhenFound()
    {
        var type = new CategoryType { CategoryTypeId = 1, Name = "T" };
        _mockRepo.Setup(r => r.GetCategoryTypeByNameAsync("T")).ReturnsAsync(type);

        var result = await _service.GetCategoryTypeByNameAsync("T");
        Assert.Equal("T", result.Name);
    }

    [Fact]
    public async Task GetAllCategoryTypesAsync_ReturnsDtos()
    {
        var list = new List<CategoryType>
        {
            new CategoryType { CategoryTypeId = 1, Name = "T1" },
        };
        _mockRepo.Setup(r => r.GetAllCategoryTypesAsync()).ReturnsAsync(list);

        var result = await _service.GetAllCategoryTypesAsync();
        Assert.Single(result);
    }

    // -----------------------
    // SubCategory Methods
    // -----------------------
    [Fact]
    public async Task GetSubCategoryByIdAsync_Throws_WhenInvalid()
    {
        await Assert.ThrowsAsync<ValidationException>(() => _service.GetSubCategoryByIdAsync(0));
    }

    [Fact]
    public async Task GetSubCategoryByIdAsync_Throws_WhenNotFound()
    {
        _mockRepo.Setup(r => r.GetSubCategoryByIdAsync(1)).ReturnsAsync((SubCategory?)null);
        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetSubCategoryByIdAsync(1));
    }

    [Fact]
    public async Task GetSubCategoryByIdAsync_ReturnsDto_WhenFound()
    {
        var sub = new SubCategory { SubCategoryId = 1, Name = "S" };
        _mockRepo.Setup(r => r.GetSubCategoryByIdAsync(1)).ReturnsAsync(sub);

        var result = await _service.GetSubCategoryByIdAsync(1);
        Assert.Equal("S", result.Name);
    }

    [Fact]
    public async Task GetSubCategoriesByCategoryAsync_Throws_WhenInvalid()
    {
        await Assert.ThrowsAsync<ValidationException>(() =>
            _service.GetSubCategoriesByCategoryAsync(0)
        );
    }

    [Fact]
    public async Task GetSubCategoriesByCategoryAsync_Throws_WhenCategoryNotFound()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Category?)null);
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.GetSubCategoriesByCategoryAsync(1)
        );
    }

    [Fact]
    public async Task GetSubCategoriesByCategoryAsync_ReturnsDtos()
    {
        var cat = new Category { CategoryId = 1 };
        var sub = new SubCategory { SubCategoryId = 1 };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(cat);
        _mockRepo.Setup(r => r.GetSubCategoriesByCategoryIdAsync(1)).ReturnsAsync(new[] { sub });

        var result = await _service.GetSubCategoriesByCategoryAsync(1);
        Assert.Single(result);
    }

    // -----------------------
    // Existence Checks
    // -----------------------
    [Fact]
    public async Task CategoryExistsAsync_ReturnsTrue_WhenExists()
    {
        _mockRepo.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
        var result = await _service.CategoryExistsAsync(1);
        Assert.True(result);
    }

    [Fact]
    public async Task CategoryExistsAsync_ByName_ReturnsTrue()
    {
        _mockRepo.Setup(r => r.ExistsAsync("X")).ReturnsAsync(true);
        var result = await _service.CategoryExistsAsync("X");
        Assert.True(result);
    }

    [Fact]
    public async Task CategoryTypeExistsAsync_ReturnsTrue_WhenExists()
    {
        _mockRepo.Setup(r => r.GetCategoryTypeByIdAsync(1)).ReturnsAsync(new CategoryType());
        var result = await _service.CategoryTypeExistsAsync(1);
        Assert.True(result);
    }

    [Fact]
    public async Task CategoryTypeExistsAsync_ByName_ReturnsTrue()
    {
        _mockRepo.Setup(r => r.GetCategoryTypeByNameAsync("T")).ReturnsAsync(new CategoryType());
        var result = await _service.CategoryTypeExistsAsync("T");
        Assert.True(result);
    }

    [Fact]
    public async Task SubCategoryExistsAsync_ReturnsTrue_WhenExists()
    {
        _mockRepo.Setup(r => r.GetSubCategoryByIdAsync(1)).ReturnsAsync(new SubCategory());
        var result = await _service.SubCategoryExistsAsync(1);
        Assert.True(result);
    }

    [Fact]
    public async Task SubCategoryExistsAsync_ByName_ReturnsTrue()
    {
        _mockRepo.Setup(r => r.SubCategoryExistsAsync("S", 1)).ReturnsAsync(true);
        var result = await _service.SubCategoryExistsAsync("S", 1);
        Assert.True(result);
    }
}
