using FluentValidation;
using SkillManager.Application.DTOs.Category;

namespace SkillManager.Application.Validators.Category
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Category name is required.")
                .MaximumLength(100)
                .WithMessage("Category name cannot exceed 100 characters.");

            RuleFor(x => x.CategoryTypeId).GreaterThan(0).WithMessage("Category type is required.");
        }
    }
}
