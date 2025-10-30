using System.Collections.Generic;
using System.Threading.Tasks;
using SkillManager.Application.DTOs.User;

namespace SkillManager.Application.Interfaces.Services
{
    public interface IUserService
    {
        // Get all users
        Task<IEnumerable<UserDto>> GetAllAsync();

        // Get single user by ID
        Task<UserDto?> GetUserByIdAsync(int userId);

        // Create a new user
        Task<(bool Success, string Message, UserDto? CreatedUser)> CreateUserAsync(
            CreateUserDto dto
        );

        // Update an existing user
        Task<(bool Success, string Message, UserDto? UpdatedUser)> UpdateUserAsync(
            UpdateUserDto dto
        );

        // Update user role separately
        Task<bool> UpdateUserRoleAsync(int userId, string roleName);
    }
}
