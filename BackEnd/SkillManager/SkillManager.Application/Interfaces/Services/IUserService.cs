using SkillManager.Application.DTOs.User;

namespace SkillManager.Application.Interfaces.Services;

public interface IUserService
{
    // Get all users as DTOs
    Task<IEnumerable<UserDto>> GetAllAsync();

    // Get a single user by ID as DTO
    Task<UserDto?> GetUserByIdAsync(int userId);

    // Admin: update UTCode and RefId
    Task<bool> UpdateUserIdentifiersAsync(int userId, string utCode, string refId);

    // Manager: update personal info, status, and delivery type
    Task<bool> UpdateUserDetailsAsync(
        int userId,
        string firstName,
        string lastName,
        string? status = null,
        string? deliveryType = null
    );
}
