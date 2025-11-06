using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillManager.Application.DTOs.User;
using SkillManager.Application.Interfaces.Services;

namespace SkillManager.Web.Pages.Users;

[Authorize(Policy = "ManagerPolicy")]
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

    // --- Bind Create / Update DTOs ---
    [BindProperty]
    public CreateUserDto CreateUserModel { get; set; } = new();

    [BindProperty]
    public UpdateUserDto UpdateUserModel { get; set; } = new();
    public int TotalUsersCount { get; set; }

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
        TotalUsersCount = allUsers.Count();
        TotalPages = (int)Math.Ceiling(TotalUsersCount / (double)PageSize);
        PageNumber = Math.Clamp(PageNumber, 1, Math.Max(1, TotalPages));

        Users = allUsers.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
    }

    // --- Save / Update Handler (AJAX-friendly) ---

    public async Task<IActionResult> OnPostSaveAsync()
    {
        // --- Validate fields manually (or rely on UpdateUserDto validation attributes) ---
        var errors = new Dictionary<string, string>();

        if (string.IsNullOrWhiteSpace(UpdateUserModel.FirstName))
            errors["firstName"] = "First Name is required";
        if (string.IsNullOrWhiteSpace(UpdateUserModel.LastName))
            errors["lastName"] = "Last Name is required";
        if (string.IsNullOrWhiteSpace(UpdateUserModel.Domain))
            errors["domain"] = "Domain is required";
        if (string.IsNullOrWhiteSpace(UpdateUserModel.Eid))
            errors["eid"] = "EID is required";

        // if errors exist and it's an AJAX request, return JSON
        if (errors.Any() && Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return new JsonResult(new { success = false, errors });
        }

        var (success, message, _) = await _userService.UpdateUserAsync(UpdateUserModel);

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            if (success)
                return new JsonResult(new { success = true, message });
            return new JsonResult(
                new { success = false, errors = new Dictionary<string, string> { { "", message } } }
            );
        }

        if (success)
            TempData["Success"] = message;
        else
        {
            ModelState.AddModelError(string.Empty, message);
            await OnGetAsync(); // re-populate Users table
            return Page();
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

    // --- Create Handler ---
    public async Task<IActionResult> OnPostCreateAsync()
    {
        var (success, message, _) = await _userService.CreateUserAsync(CreateUserModel);

        if (success)
            TempData["Success"] = message;
        else
        {
            ModelState.AddModelError(string.Empty, message);
            await OnGetAsync(); // re-populate Users table
            return Page();
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
