using FluentValidation;
using SkillManager.Application.DTOs.Category;

namespace SkillManager.Application.Validators.Category;

public class UpdateCategoryTypeValidator : AbstractValidator<UpdateCategoryTypeDto>
{
    public UpdateCategoryTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.Name))
            .WithMessage("Category type name cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Category type name cannot exceed 50 characters.");
    }
}
