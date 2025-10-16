using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillManager.Application.Abstractions.Identity;
using SkillManager.Application.Abstractions.Repository;
using SkillManager.Infrastructure.Identity.Models;

namespace SkillManager.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    // -------------------------
    // Get a single user by ID
    // -------------------------
    public async Task<User?> GetByIdAsync(string userId)
    {
        var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        return appUser == null ? null : MapToDomain(appUser);
    }

    // -------------------------
    // Get all users
    // -------------------------
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return users.Select(MapToDomain);
    }

    // -------------------------
    // Update a user
    // -------------------------
    public async Task UpdateAsync(User user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id);
        if (appUser == null)
            return;

        appUser.FirstName = user.FirstName;
        appUser.LastName = user.LastName;
        appUser.UTCode = user.UTCode;
        appUser.EmployeeId = user.EmployeeId;

        await _userManager.UpdateAsync(appUser);
    }

    // -------------------------
    // Helper: Map ApplicationUser → User
    // -------------------------
    private static User MapToDomain(ApplicationUser appUser) =>
        new()
        {
            Id = appUser.Id,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            UTCode = appUser.UTCode,
            EmployeeId = appUser.EmployeeId,
            Email = appUser.Email,
        };
}
