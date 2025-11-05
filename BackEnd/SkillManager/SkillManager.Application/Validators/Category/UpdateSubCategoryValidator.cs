using FluentValidation;
using SkillManager.Application.DTOs.SubCategory;

namespace SkillManager.Application.Validators.Category;

public class UpdateSubCategoryValidator : AbstractValidator<UpdateSubCategoryDto>
{
    public UpdateSubCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.Name))
            .WithMessage("Subcategory name cannot be empty.")
            .MaximumLength(100)
            .WithMessage("Subcategory name cannot exceed 100 characters.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .When(x => x.CategoryId.HasValue)
            .WithMessage("Category must be greater than 0.");
    }
}
