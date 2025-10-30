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
        string domain,
        string eid,
        string? status = null,
        string? deliveryType = null
    )
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return false;

        bool changed = false;

        // Update first & last name
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

        // Update Domain & Eid
        if (!string.IsNullOrWhiteSpace(domain) && user.Domain != domain)
        {
            user.Domain = domain;
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(eid) && user.Eid != eid)
        {
            user.Eid = eid;
            changed = true;
        }

        // Update status
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

        // Update delivery type
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

    public async Task<bool> CreateUserAsync(
        string firstName,
        string lastName,
        string domain,
        string eid,
        string status,
        string deliveryType,
        string utCode,
        string refId,
        string roleName
    )
    {
        var existing = await _userRepository.GetByUtCodeAsync(utCode);
        if (existing != null)
            return false; // prevent duplicate UT Codes

        var userRole = await _userRepository.GetRoleByNameAsync(roleName);
        if (userRole == null)
            return false;

        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Domain = domain,
            Eid = eid,
            Status = Enum.Parse<UserStatus>(status),
            DeliveryType = Enum.Parse<DeliveryType>(deliveryType),
            UtCode = utCode,
            RefId = refId,
            RoleId = userRole.RoleId,
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return true;
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
