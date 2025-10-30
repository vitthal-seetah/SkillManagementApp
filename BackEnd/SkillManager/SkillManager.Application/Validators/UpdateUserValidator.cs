using FluentValidation;
using SkillManager.Application.DTOs.User;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Domain.Entities;
using SkillManager.Domain.Entities.Enums;

namespace SkillManager.Application.Validators;

// ✅ UPDATE USER VALIDATOR
public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator(IUserRepository userRepository)
    {
        RuleFor(u => u.UserId).GreaterThan(0).WithMessage("Invalid user ID.");

        RuleFor(u => u.FirstName).MaximumLength(100).When(u => !string.IsNullOrEmpty(u.FirstName));

        RuleFor(u => u.LastName).MaximumLength(100).When(u => !string.IsNullOrEmpty(u.LastName));

        RuleFor(u => u.Domain).MaximumLength(100).When(u => !string.IsNullOrEmpty(u.Domain));

        RuleFor(u => u.Eid).MaximumLength(100).When(u => !string.IsNullOrEmpty(u.Eid));

        RuleFor(u => u.UtCode)
            .MaximumLength(50)
            .MustAsync(
                async (dto, utCode, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(utCode))
                        return true; // skip validation if not updating

                    var existing = await userRepository.GetByUtCodeAsync(utCode);
                    return existing == null || existing.UserId == dto.UserId;
                }
            )
            .WithMessage("UT Code must be unique.");

        RuleFor(u => u.RefId)
            .MaximumLength(100)
            .MustAsync(
                async (dto, refId, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(refId))
                        return true;

                    var existing = await userRepository.GetByRefIdAsync(refId);
                    return existing == null || existing.UserId == dto.UserId;
                }
            )
            .WithMessage("RefId must be unique.");

        RuleFor(u => u.Status)
            .Must(s => Enum.TryParse<UserStatus>(s, true, out _))
            .WithMessage("Invalid status.");

        RuleFor(u => u.DeliveryType)
            .Must(d => Enum.TryParse<DeliveryType>(d, true, out _))
            .WithMessage("Invalid delivery type.");

        RuleFor(u => u.RoleName)
            .MustAsync(
                async (roleName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(roleName))
                        return true;

                    var role = await userRepository.GetRoleByNameAsync(roleName);
                    return role != null;
                }
            )
            .WithMessage("Specified role does not exist.");
    }
}
