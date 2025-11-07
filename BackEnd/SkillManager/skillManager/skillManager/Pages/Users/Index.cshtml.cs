using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillManager.Application.DTOs.Project;
using SkillManager.Application.DTOs.User;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Domain.Entities;

namespace SkillManager.Web.Pages.Users
{
    [Authorize(Policy = "ManagerPolicy")]
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IProjectService _projectService;
        private User? _currentUserEntity;

        public IndexModel(IUserService userService, IProjectService projectService)
        {
            _userService = userService;
            _projectService = projectService;
        }

        public IEnumerable<UserDto> Users { get; set; } = new List<UserDto>();
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
        public string? SortBy { get; set; }

        // --- Pagination ---
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public int TotalUsersCount { get; set; }
        public List<ProjectDto> AvailableProjects { get; set; } = new();

        // --- Bind Create / Update DTOs ---
        [BindProperty]
        public CreateUserDto CreateUserModel { get; set; } = new();

        [BindProperty]
        public UpdateUserDto UpdateUserModel { get; set; } = new();

        private async Task<User?> GetCurrentUserAsync()
        {
            if (_currentUserEntity != null)
                return _currentUserEntity;

            // --- Extract current user's identity ---
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

            Roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            // Use the method that respects project restrictions
            _currentUserEntity = await _userService.GetUserEntityByDomainAndEidAsync(Domain, Eid);
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

            // --- Fetch all users based on current user's project ---
            var allUsers = await _userService.GetAllAsync(currentUser);

            // --- Filters ---
            if (!string.IsNullOrWhiteSpace(SelectedRole) && SelectedRole != "All")
                allUsers = allUsers.Where(u =>
                    string.Equals(u.RoleName, SelectedRole, StringComparison.OrdinalIgnoreCase)
                );

            if (!string.IsNullOrWhiteSpace(SelectedStatus) && SelectedStatus != "All")
                allUsers = allUsers.Where(u => u.Status.ToString() == SelectedStatus);

            if (!string.IsNullOrWhiteSpace(SelectedDelivery) && SelectedDelivery != "All")
                allUsers = allUsers.Where(u => u.DeliveryType.ToString() == SelectedDelivery);

            // Only show projects that the current user has access to
            AvailableProjects = (await _projectService.GetAllProjectsAsync())
                .Where(p => p.ProjectId == currentUser.ProjectId) // Only current user's project
                .ToList();

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

        // --- Create User ---
        public async Task<IActionResult> OnPostCreateAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                TempData["Error"] = "Access denied: User not found.";
                return RedirectToPage();
            }

            // Ensure new users are created in the current user's project
            CreateUserModel.ProjectId = currentUser.ProjectId;

            var (success, message, _) = await _userService.CreateUserAsync(
                CreateUserModel,
                currentUser
            );

            if (success)
                TempData["Success"] = message;
            else
                TempData["Error"] = message;

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

        // --- Save User ---
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

            var errors = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(UpdateUserModel.FirstName))
                errors["firstName"] = "First Name is required";
            if (string.IsNullOrWhiteSpace(UpdateUserModel.LastName))
                errors["lastName"] = "Last Name is required";
            if (string.IsNullOrWhiteSpace(UpdateUserModel.Domain))
                errors["domain"] = "Domain is required";
            if (string.IsNullOrWhiteSpace(UpdateUserModel.Eid))
                errors["eid"] = "EID is required";

            if (errors.Any() && Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return new JsonResult(new { success = false, errors });

            // Ensure updates stay within the current user's project
            UpdateUserModel.ProjectId = currentUser.ProjectId;

            var (success, message, _) = await _userService.UpdateUserAsync(
                UpdateUserModel,
                currentUser
            );

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return new JsonResult(new { success, message });

            if (success)
                TempData["Success"] = message;
            else
                TempData["Error"] = message;

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
}
