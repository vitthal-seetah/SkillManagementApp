using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.Abstractions.Identity;
using SkillManager.Infrastructure.Abstractions.Repository;
using SkillManager.Infrastructure.Identity.Models;
using User = SkillManager.Infrastructure.Abstractions.Identity.User;

namespace SkillManager.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    // ------------------------------------------------------
    // Get all users (Admin/Leader use case)
    // ------------------------------------------------------
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return users.Select(MapToDomain);
    }

    // ------------------------------------------------------
    // Get a single user by ID
    // ------------------------------------------------------
    public async Task<User?> GetByIdAsync(string userId)
    {
        var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        return appUser == null ? null : MapToDomain(appUser);
    }

    // ------------------------------------------------------
    // Update user (Admin/Manager use case)
    // ------------------------------------------------------
    public async Task UpdateAsync(User user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id);
        if (appUser == null)
            return;

        appUser.FirstName = user.FirstName;
        appUser.LastName = user.LastName;
        appUser.UTCode = user.UTCode;
        appUser.RefId = user.RefId;
        appUser.RoleId = user.RoleId;
        appUser.Status = user.Status;
        appUser.DeliveryType = user.DeliveryType;

        await _userManager.UpdateAsync(appUser);
    }

    // ------------------------------------------------------
    // Helper: Map ApplicationUser → Domain User
    // ------------------------------------------------------
    private static User MapToDomain(ApplicationUser appUser)
    {
        return new User
        {
            Id = appUser.Id,
            Email = appUser.Email ?? string.Empty,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            UTCode = appUser.UTCode,
            RefId = appUser.RefId,
            RoleId = appUser.RoleId,
            Status = appUser.Status,
            DeliveryType = appUser.DeliveryType,
        };
    }
}
