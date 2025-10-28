using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillManager.Application.DTOs.User;
using SkillManager.Application.Interfaces.Services;

namespace SkillManager.Web.Pages.Users;

[Authorize(Policy = "EmployeePolicy")]
public class IndexModel : PageModel
{
    private readonly IUserService _userService;

    public IndexModel(IUserService userService)
    {
        _userService = userService;
    }

    public IEnumerable<UserDto> Users { get; set; } = new List<UserDto>();
    public string Username { get; set; } = "";
    public string FullName { get; set; } = "";
    public List<string> Roles { get; set; } = new();

    public async Task OnGetAsync()
    {
        FullName = User.Identity?.Name ?? "Unavailable";
        Username = FullName.Contains('\\') ? FullName.Split('\\').Last() : FullName;
        Roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        Users = await _userService.GetAllAsync();
    }

    // Combined Save handler for Admins (updates details + identifiers + role)
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> OnPostSaveAsync(
        int userId,
        string firstName,
        string lastName,
        string status,
        string deliveryType,
        string utCode,
        string refId,
        string roleName
    )
    {
        var messages = new List<string>();

        // Update personal details
        var detailsUpdated = await _userService.UpdateUserDetailsAsync(
            userId,
            firstName,
            lastName,
            status,
            deliveryType
        );

        if (detailsUpdated)
            messages.Add("User details updated successfully.");

        // Update identifiers
        var identifiersUpdated = await _userService.UpdateUserIdentifiersAsync(
            userId,
            utCode,
            refId
        );

        if (identifiersUpdated)
            messages.Add("UT Code and Ref ID updated successfully.");

        // Update role (only if roleName provided)
        if (!string.IsNullOrWhiteSpace(roleName))
        {
            var roleUpdated = await _userService.UpdateUserRoleAsync(userId, roleName);
            if (roleUpdated)
                messages.Add($"Role updated successfully to '{roleName}'.");
        }

        if (messages.Count > 0)
        {
            TempData["Success"] = string.Join(" ", messages);
        }
        else
        {
            TempData["Error"] = "No changes were applied or update failed.";
        }

        return RedirectToPage();
    }
}
