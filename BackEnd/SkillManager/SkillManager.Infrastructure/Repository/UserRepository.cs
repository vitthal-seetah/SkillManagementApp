using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillManager.Domain.Enums;
using SkillManager.Infrastructure.Abstractions.Identity;
using SkillManager.Infrastructure.Abstractions.Repository;
using SkillManager.Infrastructure.Identity.Models;

namespace SkillManager.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<Identity.Models.ApplicationUser> _userManager;

    public UserRepository(UserManager<Identity.Models.ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    // -------------------------
    // Get a single user by ID
    // -------------------------
    public async Task<Infrastructure.Abstractions.Identity.ApplicationUser?> GetByIdAsync(
        string userId
    )
    {
        var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        return appUser == null ? null : MapToDomain(appUser);
    }

    // -------------------------
    // Get all users
    // -------------------------
    public async Task<
        IEnumerable<Infrastructure.Abstractions.Identity.ApplicationUser>
    > GetAllAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return users.Select(MapToDomain);
    }

    // -------------------------
    // Update a user
    // -------------------------
    public async Task UpdateAsync(Infrastructure.Abstractions.Identity.ApplicationUser user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id);
        if (appUser == null)
            return;

        // Map domain → persistence
        appUser.FirstName = user.FirstName;
        appUser.LastName = user.LastName;
        appUser.UTCode = user.UTCode;
        appUser.RefId = user.RefId;
        appUser.RoleId = user.RoleId;
        appUser.Status = user.Status;
        appUser.DeliveryType = user.DeliveryType;

        await _userManager.UpdateAsync(appUser);
    }

    // -------------------------
    // Helper: Map ApplicationUser → Domain User
    // -------------------------
    private static Infrastructure.Abstractions.Identity.ApplicationUser MapToDomain(
        Identity.Models.ApplicationUser appUser
    ) =>
        new()
        {
            Id = appUser.Id,
            Email = appUser.Email ?? "",
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            UTCode = appUser.UTCode,
            RefId = appUser.RefId,
            RoleId = appUser.RoleId,
            Status = appUser.Status,
            DeliveryType = appUser.DeliveryType,
        };
}
