using SkillManager.Application.DTOs.User;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Interfaces.Services;

public interface IUserService
{
    // Get all users
    Task<IEnumerable<UserDto>> GetAllAsync(User currentUser);

    // Get single user by ID
    Task<UserDto?> GetUserByIdAsync(int userId, User currentUser);

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
    Task<bool> UpdateUserRoleAsync(int userId, string roleName, User currentUser);
    Task<IEnumerable<UserDto>> GetUsersByProjectIdAsync(int projectId, User currentUser);

    Task<User?> GetUserEntityByDomainAndEidAsync(string domain, string eid, User currentUser);
    Task<UserDto?> GetUserByDomainAndEidAsync(string domain, string eid, User currentUser);
}
