using System.Security.Claims;
using System.Security.Cryptography;
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

    // --- Filtering & Sorting ---
    [BindProperty(SupportsGet = true)]
    public string? SelectedRole { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SelectedStatus { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SelectedDelivery { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SortBy { get; set; }

    // --- Pagination ---
    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalPages { get; set; }

    public async Task OnGetAsync()
    {
        FullName = User.Identity?.Name ?? "Unavailable";
        Username = FullName.Contains('\\') ? FullName.Split('\\').Last() : FullName;
        Roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

        var allUsers = await _userService.GetAllAsync();

        // --- Filters ---
        if (!string.IsNullOrWhiteSpace(SelectedRole) && SelectedRole != "All")
            allUsers = allUsers.Where(u =>
                string.Equals(u.RoleName, SelectedRole, StringComparison.OrdinalIgnoreCase)
            );

        if (!string.IsNullOrWhiteSpace(SelectedStatus) && SelectedStatus != "All")
            allUsers = allUsers.Where(u => u.Status.ToString() == SelectedStatus);

        if (!string.IsNullOrWhiteSpace(SelectedDelivery) && SelectedDelivery != "All")
            allUsers = allUsers.Where(u => u.DeliveryType.ToString() == SelectedDelivery);

        // --- Sorting ---
        allUsers = SortBy switch
        {
            "FirstNameAsc" => allUsers.OrderBy(u => u.FirstName),
            "FirstNameDesc" => allUsers.OrderByDescending(u => u.FirstName),
            "LastNameAsc" => allUsers.OrderBy(u => u.LastName),
            "LastNameDesc" => allUsers.OrderByDescending(u => u.LastName),
            "FullNameAsc" => allUsers.OrderBy(u => $"{u.FirstName} {u.LastName}"),
            "FullNameDesc" => allUsers.OrderByDescending(u => $"{u.FirstName} {u.LastName}"),
            "UTCodeAsc" => allUsers.OrderBy(u => u.UtCode),
            "UTCodeDesc" => allUsers.OrderByDescending(u => u.UtCode),
            "RoleAsc" => allUsers.OrderBy(u => u.RoleName),
            "RoleDesc" => allUsers.OrderByDescending(u => u.RoleName),
            _ => allUsers,
        };

        // --- Pagination ---
        var totalUsers = allUsers.Count();
        TotalPages = (int)Math.Ceiling(totalUsers / (double)PageSize);
        PageNumber = Math.Clamp(PageNumber, 1, Math.Max(1, TotalPages));

        Users = allUsers.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
    }

    // --- Save handler ---
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> OnPostSaveAsync(
        int userId,
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
        var messages = new List<string>();

        var detailsUpdated = await _userService.UpdateUserDetailsAsync(
            userId,
            firstName,
            lastName,
            domain,
            eid,
            status,
            deliveryType
        );
        if (detailsUpdated)
            messages.Add("User details updated successfully.");

        var identifiersUpdated = await _userService.UpdateUserIdentifiersAsync(
            userId,
            utCode,
            refId
        );
        if (identifiersUpdated)
            messages.Add("UT Code and Ref ID updated successfully.");

        if (!string.IsNullOrWhiteSpace(roleName))
        {
            var roleUpdated = await _userService.UpdateUserRoleAsync(userId, roleName);
            if (roleUpdated)
                messages.Add($"Role updated successfully to '{roleName}'.");
        }

        if (messages.Any())
            TempData["Success"] = string.Join(" ", messages);
        else
            TempData["Error"] = "No changes were applied or update failed.";

        return RedirectToPage(
            new
            {
                SelectedRole,
                SelectedStatus,
                SelectedDelivery,
                SortBy,
                PageNumber,
            }
        );
    }

    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> OnPostCreateAsync(
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
        try
        {
            var created = await _userService.CreateUserAsync(
                firstName,
                lastName,
                domain,
                eid,
                status,
                deliveryType,
                utCode,
                refId,
                roleName
            );

            if (created)
                TempData["Success"] = $"User '{firstName} {lastName}' created successfully.";
            else
                TempData["Error"] = "Failed to create user. Please check input data.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"An error occurred: {ex.Message}";
        }

        return RedirectToPage(
            new
            {
                SelectedRole,
                SelectedStatus,
                SelectedDelivery,
                SortBy,
                PageNumber,
            }
        );
    }
}
