using SkillManager.Application.DTOs.User;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Interfaces.Services;

public interface IUserService
{
    // Get all users
    Task<IEnumerable<UserDto>> GetAllAsync(User currentUser);

    // Get single user by ID
    Task<UserDto?> GetUserByIdAsync(int userId, User currentUser);

    Task<UserDto?> GetUserByDomainAndEidAsync(string domain, string eid);
    Task<User?> GetUserEntityByDomainAndEidAsync(string domain, string eid);

    // Create a new user
    Task<(bool Success, string Message, UserDto? CreatedUser)> CreateUserAsync(
        CreateUserDto dto,
        User currentUser
    );
    Task<(bool Success, string Message, UserDto? UpdatedUser)> UpdateUserAsync(
        UpdateUserDto dto,
        User currentUser
    );

    // Update user role separately
    Task<bool> UpdateUserRoleAsync(int userId, string roleName);
}
