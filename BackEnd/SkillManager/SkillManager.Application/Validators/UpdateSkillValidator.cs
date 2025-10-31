using FluentValidation;
using SkillManager.Application.DTOs.Skill;
using SkillManager.Application.Interfaces.Repositories;

namespace SkillManager.Application.Validators;

public class UpdateSkillValidator : AbstractValidator<UpdateSkillDto>
{
    public UpdateSkillValidator(
        ISkillRepository skillRepository,
        ICategoryRepository categoryRepository
    )
    {
        // Code: optional, max 50, must be unique if provided
        RuleFor(x => x.Code)
            .MaximumLength(50)
            .WithMessage("Skill code cannot exceed 50 characters.")
            .MustAsync(
                async (code, ct) =>
                {
                    if (string.IsNullOrWhiteSpace(code))
                        return true;
                    var existing = await skillRepository.GetByCodeAsync(code);
                    // If the code exists, make sure it doesn’t belong to the current skill
                    return existing == null;
                }
            )
            .WithMessage(x => $"Skill with code '{x.Code}' already exists.");

        // Label: optional, max 100
        RuleFor(x => x.Label)
            .MaximumLength(200)
            .WithMessage("Skill label cannot exceed 100 characters.");

        // CategoryId: optional, must exist
        RuleFor(x => x.CategoryId)
            .MustAsync(
                async (id, ct) =>
                    id == null || await categoryRepository.GetByIdAsync(id.Value) != null
            )
            .WithMessage(x => $"Category with ID {x.CategoryId} does not exist.");

        // SubCategoryId: optional, must exist
        RuleFor(x => x.SubCategoryId)
            .MustAsync(
                async (id, ct) =>
                    id == null || await categoryRepository.GetSubCategoryByIdAsync(id.Value) != null
            )
            .WithMessage(x => $"SubCategory with ID {x.SubCategoryId} does not exist.");

        // RequiredLevel: optional, between 1 and 4
        RuleFor(x => x.RequiredLevel)
            .InclusiveBetween(1, 4)
            .When(x => x.RequiredLevel.HasValue)
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
