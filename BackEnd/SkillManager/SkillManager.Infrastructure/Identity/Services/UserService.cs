using Microsoft.AspNetCore.Identity;
using SkillManager.Application.Abstractions.Identity;
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
        var users = await _userManager.GetUsersInRoleAsync("Employee");

        return users.Select(q => new User
        {
            Id = q.Id,
            Email = q.Email,
            FirstName = q.FirstName,
            LastName = q.LastName,
            UTCode = q.UTCode,
            EmployeeId = q.EmployeeId,
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
            EmployeeId = user.EmployeeId,
        };
    }

    // ✅ Allow updating UTCode and EmployeeId manually
    public async Task<bool> UpdateUserIdentifiersAsync(
        string userId,
        string utCode,
        string employeeId
    )
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        user.UTCode = utCode;
        user.EmployeeId = employeeId;

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }
}
