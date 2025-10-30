using FluentValidation;
using SkillManager.Application.DTOs.User;
using SkillManager.Application.Interfaces.Repositories;

namespace SkillManager.Application.Validators;

// ✅ CREATE USER VALIDATOR
public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator(IUserRepository userRepository)
    {
        RuleFor(u => u.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MaximumLength(100);

        RuleFor(u => u.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MaximumLength(100);

        RuleFor(u => u.Domain).NotEmpty().WithMessage("Domain is required.").MaximumLength(100);

        RuleFor(u => u.Eid).NotEmpty().WithMessage("EID is required.").MaximumLength(100);

        RuleFor(u => u.UtCode)
            .NotEmpty()
            .WithMessage("UT Code is required.")
            .MaximumLength(50)
            .MustAsync(
                async (utCode, cancellation) =>
                {
                    var existing = await userRepository.GetByUtCodeAsync(utCode);
                    return existing == null;
                }
            )
            .WithMessage("UT Code must be unique.");

        RuleFor(u => u.RefId)
            .NotEmpty()
            .WithMessage("RefId is required.")
            .MaximumLength(100)
            .MustAsync(
                async (refId, cancellation) =>
                {
                    var existing = await userRepository.GetByRefIdAsync(refId);
                    return existing == null;
                }
            )
            .WithMessage("RefId must be unique.");

        RuleFor(u => u.Status).IsInEnum().WithMessage("Invalid user status.");

        RuleFor(u => u.DeliveryType).IsInEnum().WithMessage("Invalid delivery type.");

        RuleFor(u => u.RoleName)
            .NotEmpty()
            .WithMessage("Role is required.")
            .MustAsync(
                async (roleName, cancellation) =>
                {
                    var role = await userRepository.GetRoleByNameAsync(roleName);
                    return role != null;
                }
            )
            .WithMessage("Specified role does not exist.");
    }
}
