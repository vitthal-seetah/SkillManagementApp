using SkillManager.Infrastructure.Abstractions.Identity;

namespace SkillManager.Application.Abstractions.Identity;

public interface IUserService
{
    Task<IEnumerable<User>> GetAll();
    Task<User?> GetUserById(string userId);
    Task<bool> UpdateUserIdentifiersAsync(string userId, string utCode, string refId);
    Task<bool> UpdateUserDetailsAsync(
        string userId,
        string firstName,
        string lastName,
        string? status = null,
        string? deliveryType = null
    );
}
