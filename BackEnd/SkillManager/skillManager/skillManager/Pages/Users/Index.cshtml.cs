using System.Security.Claims;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillManager.Application.DTOs.Project;
using SkillManager.Application.DTOs.User;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Domain.Entities;
using SkillManager.Domain.Entities.Enums;

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

    // --- Export Properties ---
    public string ManagerName { get; set; } = "";
    public string ProjectName { get; set; } = "";

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

        // Apply filters and sorting
        var filteredUsers = ApplyFilters(allUsers);
        var sortedUsers = ApplySorting(filteredUsers);

        // --- Pagination ---
        TotalUsersCount = sortedUsers.Count();
        TotalPages = (int)Math.Ceiling(TotalUsersCount / (double)PageSize);
        PageNumber = Math.Clamp(PageNumber, 1, Math.Max(1, TotalPages));

        Users = sortedUsers.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
    }

    // --- NEW: ApplyFilters method ---
    private IEnumerable<UserDto> ApplyFilters(IEnumerable<UserDto> users)
    {
        var filtered = users;

        if (!string.IsNullOrEmpty(SelectedRole) && SelectedRole != "All")
        {
            filtered = filtered.Where(u =>
                string.Equals(u.RoleName, SelectedRole, StringComparison.OrdinalIgnoreCase)
            );
        }

        if (!string.IsNullOrEmpty(SelectedStatus) && SelectedStatus != "All")
        {
            if (Enum.TryParse<UserStatus>(SelectedStatus, out var status))
            {
                filtered = filtered.Where(u => u.Status == status);
            }
        }

        if (!string.IsNullOrEmpty(SelectedDelivery) && SelectedDelivery != "All")
        {
            if (Enum.TryParse<DeliveryType>(SelectedDelivery, out var deliveryType))
            {
                filtered = filtered.Where(u => u.DeliveryType == deliveryType);
            }
        }

        if (!string.IsNullOrEmpty(SelectedTeam) && SelectedTeam != "All")
        {
            if (int.TryParse(SelectedTeam, out var teamId))
            {
                filtered = filtered.Where(u => u.TeamId == teamId);
            }
        }

        return filtered;
    }

    // --- NEW: ApplySorting method ---
    private IEnumerable<UserDto> ApplySorting(IEnumerable<UserDto> users)
    {
        return SortBy switch
        {
            "FirstNameAsc" => users.OrderBy(u => u.FirstName),
            "FirstNameDesc" => users.OrderByDescending(u => u.FirstName),
            "LastNameAsc" => users.OrderBy(u => u.LastName),
            "LastNameDesc" => users.OrderByDescending(u => u.LastName),
            "FullNameAsc" => users.OrderBy(u => $"{u.FirstName} {u.LastName}"),
            "FullNameDesc" => users.OrderByDescending(u => $"{u.FirstName} {u.LastName}"),
            "UTCodeAsc" => users.OrderBy(u => u.UtCode),
            "UTCodeDesc" => users.OrderByDescending(u => u.UtCode),
            "RoleAsc" => users.OrderBy(u => u.RoleName),
            "RoleDesc" => users.OrderByDescending(u => u.RoleName),
            "TeamAsc" => users.OrderBy(u => u.TeamName ?? ""),
            "TeamDesc" => users.OrderByDescending(u => u.TeamName ?? ""),
            _ => users.OrderBy(u => u.FirstName).ThenBy(u => u.LastName),
        };
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
        if (UpdateUserModel.TeamId != null && UpdateUserModel.TeamId != 0)
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

    // --- Export Handler ---
    public async Task<IActionResult> OnGetExportAsync()
    {
        Console.WriteLine("Excel export function launched for User Management");

        var currentUser = await GetCurrentUserAsync();
        if (currentUser == null)
        {
            return RedirectToPage("/Error");
        }

        ManagerName = $"{currentUser.FirstName} {currentUser.LastName}";
        ProjectName = currentUser.Project?.ProjectName ?? "Current Project";

        // Get all users with current filters applied
        var allUsers = await _userService.GetAllAsync(currentUser);
        var filteredUsers = ApplyFilters(allUsers);
        var sortedUsers = ApplySorting(filteredUsers);

        // Generate Excel
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Users");

        // Add title with project info
        ws.Cell(1, 1).Value = $"User Management Report - {ProjectName} - {DateTime.Now:yyyy-MM-dd}";
        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontSize = 16;
        ws.Range(1, 1, 1, 12).Merge();

        // Add filters info
        ws.Cell(2, 1).Value = $"Project: {ProjectName}";
        ws.Cell(3, 1).Value = $"Generated By: {ManagerName}";
        ws.Cell(4, 1).Value = $"Role Filter: {SelectedRole ?? "All"}";
        ws.Cell(5, 1).Value = $"Status Filter: {SelectedStatus ?? "All"}";
        ws.Cell(6, 1).Value = $"Team Filter: {SelectedTeam ?? "All"}";
        ws.Cell(7, 1).Value = $"Delivery Type Filter: {SelectedDelivery ?? "All"}";
        ws.Cell(8, 1).Value = $"Total Users: {sortedUsers.Count()}";

        // Headers starting from row 10
        int headerRow = 10;
        ws.Cell(headerRow, 1).Value = "User ID";
        ws.Cell(headerRow, 2).Value = "First Name";
        ws.Cell(headerRow, 3).Value = "Last Name";
        ws.Cell(headerRow, 4).Value = "Domain";
        ws.Cell(headerRow, 5).Value = "EID";
        ws.Cell(headerRow, 6).Value = "Status";
        ws.Cell(headerRow, 7).Value = "Delivery Type";
        ws.Cell(headerRow, 8).Value = "UT Code";
        ws.Cell(headerRow, 9).Value = "Ref ID";
        ws.Cell(headerRow, 10).Value = "Role";
        ws.Cell(headerRow, 11).Value = "Project";
        ws.Cell(headerRow, 12).Value = "Team";

        // Style headers
        var headerRange = ws.Range(headerRow, 1, headerRow, 12);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        // Data rows
        int row = headerRow + 1;
        foreach (var user in sortedUsers)
        {
            ws.Cell(row, 1).Value = user.UserId;
            ws.Cell(row, 2).Value = user.FirstName;
            ws.Cell(row, 3).Value = user.LastName;
            ws.Cell(row, 4).Value = user.Domain;
            ws.Cell(row, 5).Value = user.Eid;
            ws.Cell(row, 6).Value = user.Status.ToString();
            ws.Cell(row, 7).Value = user.DeliveryType.ToString();
            ws.Cell(row, 8).Value = user.UtCode;
            ws.Cell(row, 9).Value = user.RefId;
            ws.Cell(row, 10).Value = user.RoleName;
            ws.Cell(row, 11).Value = user.ProjectName;
            ws.Cell(row, 12).Value = user.TeamName;
            row++;
        }

        // Add summary statistics
        int summaryRow = row + 2;
        ws.Cell(summaryRow, 1).Value = "Summary Statistics";
        ws.Cell(summaryRow, 1).Style.Font.Bold = true;
        ws.Cell(summaryRow, 1).Style.Font.FontSize = 14;

        ws.Cell(summaryRow + 1, 1).Value = "Total Users:";
        ws.Cell(summaryRow + 1, 2).Value = sortedUsers.Count();

        // Role distribution
        var roleDistribution = sortedUsers
            .GroupBy(u => u.RoleName)
            .ToDictionary(g => g.Key, g => g.Count());

        int roleRow = summaryRow + 3;
        ws.Cell(roleRow, 1).Value = "Role Distribution";
        ws.Cell(roleRow, 1).Style.Font.Bold = true;

        foreach (var role in roleDistribution)
        {
            roleRow++;
            ws.Cell(roleRow, 1).Value = $"{role.Key}:";
            ws.Cell(roleRow, 2).Value = role.Value;
        }

        // Format columns
        ws.Columns().AdjustToContents();

        // Create the file stream
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        var fileName = $"UserManagement_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

        return File(
            stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName
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
