using FluentValidation;
using SkillManager.Application.DTOs.Category;

namespace SkillManager.Application.Validators.Category;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.Name))
            .WithMessage("Category name cannot be empty.")
            .MaximumLength(100)
            .WithMessage("Category name cannot exceed 100 characters.");

        RuleFor(x => x.CategoryTypeId)
            .GreaterThan(0)
            .When(x => x.CategoryTypeId.HasValue)
            .WithMessage("Category type must be greater than 0.");
    }
}
