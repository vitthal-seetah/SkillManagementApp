using Microsoft.AspNetCore.Identity;
using SkillManager.Application.Abstractions.Identity;
using SkillManager.Domain.Entities;
using SkillManager.Domain.Enums;
using SkillManager.Infrastructure.Identity.Models;

namespace SkillManager.Infrastructure.Identity.Services;

public sealed class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        // Get all users regardless of role
        var users = _userManager.Users.ToList();

        return users.Select(q => new User
        {
            Id = q.Id,
            Email = q.Email,
            FirstName = q.FirstName,
            LastName = q.LastName,
            UTCode = q.UTCode,
            RefId = q.RefId,
            RoleId = q.RoleId,
            Status = q.Status,
            DeliveryType = q.DeliveryType,
        });
    }

    public async Task<User?> GetUserById(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return null;

        return new User
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UTCode = user.UTCode,
            RefId = user.RefId,
            RoleId = user.RoleId,
            Status = user.Status,
            DeliveryType = user.DeliveryType,
        };
    }

    // Admin: update ID-related fields (UTCode, RefId)
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

    // Manager: update other fields (FirstName, LastName, Status, DeliveryType)
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
}
