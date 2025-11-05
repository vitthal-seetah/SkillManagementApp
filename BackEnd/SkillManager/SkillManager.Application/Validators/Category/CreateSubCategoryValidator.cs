using FluentValidation;
using SkillManager.Application.DTOs.SubCategory;

namespace SkillManager.Application.Validators.Category
{
    public class CreateSubCategoryValidator : AbstractValidator<CreateSubCategoryDto>
    {
        public CreateSubCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Subcategory name is required.")
                .MaximumLength(100)
                .WithMessage("Subcategory name cannot exceed 100 characters.");

            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Category is required.");
        }
    }
}
