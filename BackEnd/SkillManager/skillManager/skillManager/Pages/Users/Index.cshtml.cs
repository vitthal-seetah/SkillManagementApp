using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillManager.Application.DTOs.Project;
using SkillManager.Application.DTOs.User;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Domain.Entities;

namespace SkillManager.Web.Pages.Users;

[Authorize(Policy = "EmployeePolicy")]
public class IndexModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ITeamService _teamService;
    private readonly IProjectService _projectService;
    private User? _currentUserEntity;

    public IndexModel(
        IUserService userService,
        ITeamService teamService,
        IProjectService projectService
    )
    {
        _userService = userService;
        _teamService = teamService;
        _projectService = projectService;
    }

    public IEnumerable<UserDto> Users { get; set; } = new List<UserDto>();
    public string Username { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Domain { get; set; } = "";
    public string Eid { get; set; } = "";
    public List<string> Roles { get; set; } = new();

    // --- Filtering & Sorting ---
    [BindProperty(SupportsGet = true)]
    public string? SelectedRole { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SelectedStatus { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SelectedDelivery { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SelectedTeam { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SortBy { get; set; }

    // --- Pagination ---
    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalPages { get; set; }

    public List<Team> AvailableTeams { get; set; } = new();
    public List<ProjectDto> AvailableProjects { get; set; } = new();

    // --- Bind Create / Update DTOs ---
    [BindProperty]
    public CreateUserDto CreateUserModel { get; set; } = new();

    [BindProperty]
    public UpdateUserDto UpdateUserModel { get; set; } = new();
    public int TotalUsersCount { get; set; }

    private async Task<User?> GetCurrentUserAsync()
    {
        if (_currentUserEntity != null)
            return _currentUserEntity;

        // Extract current user's identity
        FullName = User.Identity?.Name ?? "Unavailable";

        if (FullName.Contains('\\'))
        {
            var parts = FullName.Split('\\', 2);
            Domain = parts[0];
            Eid = parts[1];
        }
        else
        {
            Domain = "";
            Eid = FullName;
        }

        Username = Eid;
        Roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

        // Get current user entity with project information
        _currentUserEntity = await _userService.GetUserEntityByDomainAndEidAsync(Domain, Eid, null);
        return _currentUserEntity;
    }

    public async Task OnGetAsync()
    {
        var currentUser = await GetCurrentUserAsync();
        if (currentUser == null)
        {
            Users = new List<UserDto>();
            TempData["Error"] = "User not found or access denied.";
            return;
        }

        // Get users only from current user's project
        var allUsers = await _userService.GetAllAsync(currentUser);

        // FIX: Use the new method that includes ProjectTeams
        var allTeams = await _teamService.GetAllTeamsWithProjectsAsync();
        AvailableTeams = allTeams
            .Where(t => t.ProjectTeams?.Any(pt => pt.ProjectId == currentUser.ProjectId) == true)
            .ToList();

        // Get available projects (only current user's project for non-admins)
        AvailableProjects = (await _projectService.GetAllProjectsAsync())
            .Where(p => p.ProjectId == currentUser.ProjectId)
            .ToList();

        // --- Filters ---
        if (!string.IsNullOrWhiteSpace(SelectedRole) && SelectedRole != "All")
            allUsers = allUsers.Where(u =>
                string.Equals(u.RoleName, SelectedRole, StringComparison.OrdinalIgnoreCase)
            );

        if (!string.IsNullOrWhiteSpace(SelectedStatus) && SelectedStatus != "All")
            allUsers = allUsers.Where(u => u.Status.ToString() == SelectedStatus);

        if (!string.IsNullOrWhiteSpace(SelectedDelivery) && SelectedDelivery != "All")
            allUsers = allUsers.Where(u => u.DeliveryType.ToString() == SelectedDelivery);

        if (!string.IsNullOrWhiteSpace(SelectedTeam) && SelectedTeam != "All")
        {
            if (int.TryParse(SelectedTeam, out int teamId))
            {
                allUsers = allUsers.Where(u => u.TeamId == teamId);
            }
        }

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
            // Add Team sorting
            "TeamAsc" => allUsers.OrderBy(u => u.TeamName ?? ""),
            "TeamDesc" => allUsers.OrderByDescending(u => u.TeamName ?? ""),
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
        var currentUser = await GetCurrentUserAsync();
        if (currentUser == null)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return new JsonResult(
                    new { success = false, message = "Access denied: User not found." }
                );

            TempData["Error"] = "Access denied: User not found.";
            return RedirectToPage();
        }

        // --- Validate fields manually ---
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

        // Ensure updates stay within current user's project
        UpdateUserModel.ProjectId = currentUser.ProjectId;

        // FIX: Use the new method for team validation
        if (UpdateUserModel.TeamId != null || UpdateUserModel.TeamId != 0)
        {
            var availableTeams = await _teamService.GetAllTeamsWithProjectsAsync();
            var team = availableTeams.FirstOrDefault(t => t.TeamId == UpdateUserModel.TeamId);

            // Check if team is associated with current user's project through ProjectTeam
            if (
                team == null
                || team.ProjectTeams?.Any(pt => pt.ProjectId == currentUser.ProjectId) != true
            )
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return new JsonResult(
                        new { success = false, message = "Invalid team selection." }
                    );

                TempData["Error"] = "Invalid team selection.";
                return RedirectToPage();
            }
        }

        var (success, message, _) = await _userService.UpdateUserAsync(
            UpdateUserModel,
            currentUser
        );

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
                SelectedTeam,
                SortBy,
                PageNumber,
            }
        );
    }

    // --- Create Handler ---
    public async Task<IActionResult> OnPostCreateAsync()
    {
        var currentUser = await GetCurrentUserAsync();
        if (currentUser == null)
        {
            TempData["Error"] = "Access denied: User not found.";
            return RedirectToPage();
        }

        // Ensure new users are created in current user's project
        CreateUserModel.ProjectId = currentUser.ProjectId;

        // FIX: Use the new method for team validation
        if (CreateUserModel.TeamId.HasValue)
        {
            var availableTeams = await _teamService.GetAllTeamsWithProjectsAsync();
            var team = availableTeams.FirstOrDefault(t => t.TeamId == CreateUserModel.TeamId.Value);

            // Check if team is associated with current user's project through ProjectTeam
            if (
                team == null
                || team.ProjectTeams?.Any(pt => pt.ProjectId == currentUser.ProjectId) != true
            )
            {
                TempData["Error"] = "Invalid team selection.";
                return RedirectToPage();
            }
        }

        var (success, message, _) = await _userService.CreateUserAsync(
            CreateUserModel,
            currentUser
        );

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
                SelectedTeam,
                SortBy,
                PageNumber,
            }
        );
    }
}
