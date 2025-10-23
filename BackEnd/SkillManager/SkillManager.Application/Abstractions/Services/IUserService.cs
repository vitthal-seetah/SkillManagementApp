using SkillManager.Domain.Entities;

namespace SkillManager.Application.Abstractions.Identity;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetUserByIdAsync(int userId);
    Task<bool> UpdateUserIdentifiersAsync(int userId, string utCode, string refId);
    Task<bool> UpdateUserDetailsAsync(
        int userId,
        string firstName,
        string lastName,
        string? status = null,
        string? deliveryType = null
    );
}
