using FluentValidation;
using SkillManager.Application.DTOs.Category;

namespace SkillManager.Application.Validators;

public class CreateCategoryTypeValidator : AbstractValidator<CreateCategoryTypeDto>
{
    public CreateCategoryTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Category type name is required.")
            .MaximumLength(50)
            .WithMessage("Category type name cannot exceed 50 characters.");
    }
}
