using FluentValidation;
using SkillManager.Application.DTOs.Skill;
using SkillManager.Application.Interfaces.Repositories;

namespace SkillManager.Application.Validators;

public class CreateSkillValidator : AbstractValidator<CreateSkillDto>
{
    public CreateSkillValidator(
        ISkillRepository skillRepository,
        ICategoryRepository categoryRepository
    )
    {
        // Skill code: required, max 50 chars, must be unique
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Skill code is required.")
            .MaximumLength(50)
            .WithMessage("Skill code cannot exceed 50 characters.")
            .MustAsync(async (code, ct) => await skillRepository.GetByCodeAsync(code) == null)
            .WithMessage(x => $"Skill with code '{x.Code}' already exists.");

        // Label: required, max 100 chars
        RuleFor(x => x.Label)
            .NotEmpty()
            .WithMessage("Skill label is required.")
            .MaximumLength(100)
            .WithMessage("Skill label cannot exceed 100 characters.");

        // Category: must exist
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category is required.")
            .MustAsync(async (id, ct) => await categoryRepository.GetByIdAsync(id) != null)
            .WithMessage(x => $"Category with ID {x.CategoryId} does not exist.");

        // SubCategory: must exist
        RuleFor(x => x.SubCategoryId)
            .NotEmpty()
            .WithMessage("SubCategory is required.")
            .MustAsync(
                async (id, ct) => await categoryRepository.GetSubCategoryByIdAsync(id) != null
            )
            .WithMessage(x => $"SubCategory with ID {x.SubCategoryId} does not exist.");

        // RequiredLevel: between 1 and 4
        RuleFor(x => x.RequiredLevel)
            .InclusiveBetween(1, 4)
            .WithMessage("Required level must be between 1 and 4.");

        // CriticalityLevel: optional, max 50
        RuleFor(x => x.CriticalityLevel)
            .MaximumLength(50)
            .WithMessage("Criticality level cannot exceed 50 characters.");

        // Level descriptions: optional, max 500
        RuleFor(x => x.FirstLevelDescription).MaximumLength(500);
        RuleFor(x => x.SecondLevelDescription).MaximumLength(500);
        RuleFor(x => x.ThirdLevelDescription).MaximumLength(500);
        RuleFor(x => x.FourthLevelDescription).MaximumLength(500);
    }
}
