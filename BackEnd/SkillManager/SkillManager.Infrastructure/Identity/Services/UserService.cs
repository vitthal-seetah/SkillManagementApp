using Microsoft.AspNetCore.Identity;
using SkillManager.Application.Abstractions.Identity;
using SkillManager.Domain.Entities;
using SkillManager.Domain.Entities.Enums;
using SkillManager.Infrastructure.Abstractions.Identity;
using SkillManager.Infrastructure.Identity.Models;
using User = SkillManager.Infrastructure.Abstractions.Identity.User;

namespace SkillManager.Infrastructure.Identity.Services;

public sealed class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    // -----------------------------
    // Get all users
    // -----------------------------
    public async Task<IEnumerable<User>> GetAll()
    {
        var users = await Task.FromResult(_userManager.Users.ToList());

        return users.Select(u => MapToDomain(u));
    }

    // -----------------------------
    // Get single user by ID
    // -----------------------------
    public async Task<User?> GetUserById(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user == null ? null : MapToDomain(user);
    }

    // -----------------------------
    // Admin: Update user identifiers (UTCode, RefId)
    // -----------------------------
    public async Task<bool> UpdateUserIdentifiersAsync(string userId, string utCode, string refId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        user.UTCode = utCode;
        user.RefId = refId;

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    // -----------------------------
    // Manager: Update personal or status info
    // -----------------------------
    public async Task<bool> UpdateUserDetailsAsync(
        string userId,
        string firstName,
        string lastName,
        string? status = null,
        string? deliveryType = null
    )
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        user.FirstName = firstName;
        user.LastName = lastName;

        // Parse enum values if provided
        if (
            !string.IsNullOrEmpty(status)
            && Enum.TryParse(status, true, out UserStatus parsedStatus)
        )
            user.Status = parsedStatus;

        if (
            !string.IsNullOrEmpty(deliveryType)
            && Enum.TryParse(deliveryType, true, out DeliveryType parsedDelivery)
        )
            user.DeliveryType = parsedDelivery;

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    // -----------------------------
    // Helper: Map ApplicationUser -> Domain User
    // -----------------------------
    private static Abstractions.Identity.User MapToDomain(ApplicationUser u)
    {
        return new Abstractions.Identity.User
        {
            Id = u.Id,
            Email = u.Email ?? string.Empty,
            FirstName = u.FirstName,
            LastName = u.LastName,
            UTCode = u.UTCode,
            RefId = u.RefId,
            Status = u.Status,
            DeliveryType = u.DeliveryType,
        };
    }
}
