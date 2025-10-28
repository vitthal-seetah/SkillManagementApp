using System.Data;
using SkillManager.Application.DTOs.User;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Domain.Entities;
using SkillManager.Domain.Entities.Enums;

namespace SkillManager.Application.Services;

public sealed class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // -----------------------------
    // Get all users (mapped to DTO)
    // -----------------------------
    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(u => MapToDto(u));
    }

    // -----------------------------
    // Get single user by ID (mapped to DTO)
    // -----------------------------
    public async Task<UserDto?> GetUserByIdAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return user == null ? null : MapToDto(user);
    }

    // -----------------------------
    // Admin: Update UTCode and RefId
    // -----------------------------
    public async Task<bool> UpdateUserIdentifiersAsync(int userId, string utCode, string refId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return false;

        bool changed = false;

        if (user.UtCode != utCode)
        {
            user.UtCode = utCode;
            changed = true;
        }

        if (user.RefId != refId)
        {
            user.RefId = refId;
            changed = true;
        }

        if (changed)
            await _userRepository.UpdateAsync(user);

        return changed;
    }

    // -----------------------------
    // Manager/Admin: Update personal info and status/delivery
    // -----------------------------
    public async Task<bool> UpdateUserDetailsAsync(
        int userId,
        string firstName,
        string lastName,
        string? status = null,
        string? deliveryType = null
    )
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return false;

        bool changed = false;

        if (user.FirstName != firstName)
        {
            user.FirstName = firstName;
            changed = true;
        }

        if (user.LastName != lastName)
        {
            user.LastName = lastName;
            changed = true;
        }

        if (
            !string.IsNullOrEmpty(status)
            && Enum.TryParse(status, true, out UserStatus parsedStatus)
        )
        {
            if (user.Status != parsedStatus)
            {
                user.Status = parsedStatus;
                changed = true;
            }
        }

        if (
            !string.IsNullOrEmpty(deliveryType)
            && Enum.TryParse(deliveryType, true, out DeliveryType parsedDelivery)
        )
        {
            if (user.DeliveryType != parsedDelivery)
            {
                user.DeliveryType = parsedDelivery;
                changed = true;
            }
        }

        if (changed)
            await _userRepository.UpdateAsync(user);

        return changed;
    }

    // -----------------------------
    // Admin: Update role
    // -----------------------------
    public async Task<bool> UpdateUserRoleAsync(int userId, string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return false;

        var role = await _userRepository.GetRoleByNameAsync(roleName);
        if (role == null)
            return false;

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return false;

        if (user.RoleId == role.RoleId)
            return false; // no change

        user.RoleId = role.RoleId;
        await _userRepository.UpdateAsync(user);

        // Reload user to ensure Role navigation property is updated
        var updatedUser = await _userRepository.GetByIdAsync(userId);
        return updatedUser.RoleId == role.RoleId;
    }

    // -----------------------------
    // Private helper: map User -> UserDto
    // -----------------------------
    private static UserDto MapToDto(User u)
    {
        return new UserDto
        {
            UserId = u.UserId,
            FirstName = u.FirstName,
            LastName = u.LastName,
            UtCode = u.UtCode,
            RefId = u.RefId ?? string.Empty,
            RoleName = u.Role?.Name ?? string.Empty, // correctly included
            Domain = u.Domain,
            Eid = u.Eid,
            Status = u.Status,
            DeliveryType = u.DeliveryType,
        };
    }
}
