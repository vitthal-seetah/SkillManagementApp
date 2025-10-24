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

        user.UtCode = utCode;
        user.RefId = refId;

        await _userRepository.UpdateAsync(user);
        return true;
    }

    // -----------------------------
    // Manager: Update personal info and status/delivery
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

        user.FirstName = firstName;
        user.LastName = lastName;

        if (
            !string.IsNullOrEmpty(status)
            && Enum.TryParse(status, true, out UserStatus parsedStatus)
        )
        {
            user.Status = parsedStatus;
        }

        if (
            !string.IsNullOrEmpty(deliveryType)
            && Enum.TryParse(deliveryType, true, out DeliveryType parsedDelivery)
        )
        {
            user.DeliveryType = parsedDelivery;
        }

        await _userRepository.UpdateAsync(user);
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
            RoleName = u.Role?.Name ?? string.Empty,
            Domain = u.Domain,
            Eid = u.Eid,
            Status = u.Status,
            DeliveryType = u.DeliveryType,
        };
    }
}
