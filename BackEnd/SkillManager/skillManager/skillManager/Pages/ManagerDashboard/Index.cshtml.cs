using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Application.Services;
using SkillManager.Domain.Entities;

namespace SkillManager.Web.Pages
{
    [Authorize(Policy = "ManagerPolicy")]
    public class ManagerDashboardModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IUserSkillService _userSkillService;
        private readonly ICategoryService _categoryService;
        private readonly ITeamService _teamService;
        private User? _currentUserEntity;

        public ManagerDashboardModel(
            IUserService userService,
            IUserSkillService userSkillService,
            ICategoryService categoryService,
            ITeamService teamService
        )
        {
            _userService = userService;
            _userSkillService = userSkillService;
            _categoryService = categoryService;
            _teamService = teamService;
        }

        public List<UserSummary> UserSummaries { get; set; } = new();
        public List<UserSummary> AllUserSummaries { get; set; } = new(); // For charts and analytics
        public string FullName { get; set; } = "";
        public string Domain { get; set; } = "";
        public string Eid { get; set; } = "";
        public List<string> Roles { get; set; } = new();
        public Dictionary<string, string> CategoryColors { get; set; } = new();
        public Dictionary<string, string> RoleColors { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SortBy { get; set; }

        // New properties for header information
        public string ManagerTeamName { get; set; } = "";
        public int TeammateCount { get; set; }
        public string ProjectName { get; set; } = "";
        public int NumberOfTeams { get; set; }

        // Pagination properties
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public int TotalUsersCount { get; set; }

        private async Task<User?> GetCurrentUserAsync()
        {
            if (_currentUserEntity != null)
                return _currentUserEntity;

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

            // Use the same approach as Team/IndexModel to get current user entity
            var currentUserId = GetCurrentUserId();
            if (currentUserId > 0)
            {
                _currentUserEntity = await _userService.GetUserEntityByIdAsync(currentUserId);
            }
            else
            {
                // Fallback to domain/eid lookup
                _currentUserEntity = await _userService.GetUserEntityByDomainAndEidAsync(
                    Domain,
                    Eid,
                    null
                );
            }

            return _currentUserEntity;
        }

        private int GetCurrentUserId()
        {
            var uidClaim = User.FindFirst("uid");
            if (uidClaim != null && int.TryParse(uidClaim.Value, out int userId))
            {
                return userId;
            }
            return 0;
        }

        public async Task OnGetAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                TempData["Error"] = "User not found or access denied.";
                return;
            }

            // Check if user has a project assigned
            if (!currentUser.ProjectId.HasValue)
            {
                TempData["Error"] =
                    "You are not assigned to any project. Please contact an administrator.";
                return;
            }
            var teams = await _teamService.GetTeamsByProjectIdAsync(currentUser.ProjectId);
            NumberOfTeams = teams.Count();

            // FIX: Get ALL users in the same PROJECT, not just the same team
            var allUsers = await _userService.GetAllAsync(currentUser);

            // DEBUG: Check what we're getting
            Console.WriteLine($"=== MANAGER DASHBOARD DEBUG ===");
            Console.WriteLine($"Current User: {currentUser.FirstName} {currentUser.LastName}");
            Console.WriteLine($"Current User ProjectId: {currentUser.ProjectId}");
            Console.WriteLine($"Current User TeamId: {currentUser.TeamId}");
            Console.WriteLine($"Total users from service: {allUsers.Count()}");

            // Filter to only users in the same project (additional safety check)
            var projectUsers = allUsers.Where(u => u.ProjectId == currentUser.ProjectId).ToList();
            Console.WriteLine($"Users in same project: {projectUsers.Count}");

            foreach (var user in projectUsers)
            {
                Console.WriteLine(
                    $"- {user.FirstName} {user.LastName} | Project: {user.ProjectId} | Team: {user.TeamId}"
                );
            }
            Console.WriteLine($"=== END DEBUG ===");

            // Set header information
            ProjectName = currentUser.Project?.ProjectName ?? "No Project";
            ManagerTeamName = currentUser.Team?.TeamName ?? "No Team";

            // Count teammates (users in the same project, excluding the manager)
            TotalUsersCount = projectUsers.Count(u => u.UserId != currentUser.UserId);
            TeammateCount = TotalUsersCount;

            // Get actual categories from database
            var categories = await _categoryService.GetAllCategoriesAsync();
            var colorPalette = new[]
            {
                "#4e79a7",
                "#f28e2b",
                "#e15759",
                "#76b7b2",
                "#59a14f",
                "#edc948",
                "#b07aa1",
                "#ff9da7",
                "#9c755f",
                "#bab0ac",
            };

            CategoryColors = categories
                .Select(
                    (category, index) =>
                        new { category.Name, Color = colorPalette[index % colorPalette.Length] }
                )
                .ToDictionary(x => x.Name, x => x.Color);

            // Get skill summaries for each user
            var allUserSummaries = new List<UserSummary>();

            foreach (var user in projectUsers)
            {
                var userSkills = await _userSkillService.GetUserSkillsByUserIdAsync(user.UserId);

                // Calculate average level points by category
                var categoryAverages = userSkills
                    .GroupBy(s => s.CategoryName ?? "Uncategorized")
                    .ToDictionary(g => g.Key, g => g.Average(s => s.LevelId));

                var summary = new UserSummary
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UtCode = user.UtCode,
                    ProjectName = user.ProjectName,
                    TeamName = user.TeamName,
                    RoleName = user.RoleName,
                    CategoryAverages = categoryAverages,
                    TotalSkills = userSkills.Count(),
                    OverallAverage = userSkills.Any() ? userSkills.Average(s => s.LevelId) : 0,
                    LastUpdated = await _userSkillService.GetLastUpdatedTimeAsync(user.UserId),
                };

                allUserSummaries.Add(summary);
            }

            AllUserSummaries = allUserSummaries;

            // Set up fixed colors for roles
            var distinctRoles = allUserSummaries
                .Select(u => u.RoleName)
                .Distinct()
                .OrderBy(r => r)
                .ToList();
            var roleColorPalette = new[]
            {
                "#4e79a7",
                "#f28e2b",
                "#e15759",
                "#76b7b2",
                "#59a14f",
                "#edc948",
                "#b07aa1",
                "#ff9da7",
                "#9c755f",
                "#bab0ac",
                "#5fa4d4",
                "#ffb55a",
                "#ff7f7f",
                "#8cd4c8",
                "#7bb76d",
            };

            RoleColors = distinctRoles
                .Select(
                    (role, index) =>
                        new
                        {
                            Role = role,
                            Color = roleColorPalette[index % roleColorPalette.Length],
                        }
                )
                .ToDictionary(x => x.Role, x => x.Color);

            // Apply Sorting and Pagination
            ApplySortingAndPagination();
        }

        private void ApplySortingAndPagination()
        {
            // Apply sorting to all summaries
            var sortedSummaries = SortBy switch
            {
                "UserAsc" => AllUserSummaries
                    .OrderBy(u => u.FirstName)
                    .ThenBy(u => u.LastName)
                    .ToList(),
                "UserDesc" => AllUserSummaries
                    .OrderByDescending(u => u.FirstName)
                    .ThenByDescending(u => u.LastName)
                    .ToList(),
                "UtCodeAsc" => AllUserSummaries.OrderBy(u => u.UtCode).ToList(),
                "UtCodeDesc" => AllUserSummaries.OrderByDescending(u => u.UtCode).ToList(),
                "RoleAsc" => AllUserSummaries.OrderBy(u => u.RoleName).ToList(),
                "RoleDesc" => AllUserSummaries.OrderByDescending(u => u.RoleName).ToList(),
                "TeamAsc" => AllUserSummaries.OrderBy(u => u.TeamName ?? "").ToList(),
                "TeamDesc" => AllUserSummaries.OrderByDescending(u => u.TeamName ?? "").ToList(),
                "TotalSkillsAsc" => AllUserSummaries.OrderBy(u => u.TotalSkills).ToList(),
                "TotalSkillsDesc" => AllUserSummaries
                    .OrderByDescending(u => u.TotalSkills)
                    .ToList(),
                "OverallAverageAsc" => AllUserSummaries.OrderBy(u => u.OverallAverage).ToList(),
                "OverallAverageDesc" => AllUserSummaries
                    .OrderByDescending(u => u.OverallAverage)
                    .ToList(),
                "LastUpdatedAsc" => AllUserSummaries
                    .OrderBy(u => u.LastUpdated ?? DateTime.MinValue)
                    .ToList(),
                "LastUpdatedDesc" => AllUserSummaries
                    .OrderByDescending(u => u.LastUpdated ?? DateTime.MinValue)
                    .ToList(),
                _ => AllUserSummaries.OrderByDescending(u => u.OverallAverage).ToList(), // Default sort
            };

            // Handle category sorting
            if (!string.IsNullOrEmpty(SortBy) && CategoryColors.Keys.Any(c => SortBy.StartsWith(c)))
            {
                var category = CategoryColors.Keys.First(c => SortBy.StartsWith(c));
                if (SortBy.EndsWith("Asc"))
                {
                    sortedSummaries = sortedSummaries
                        .OrderBy(u =>
                            u.CategoryAverages.ContainsKey(category)
                                ? u.CategoryAverages[category]
                                : 0
                        )
                        .ToList();
                }
                else if (SortBy.EndsWith("Desc"))
                {
                    sortedSummaries = sortedSummaries
                        .OrderByDescending(u =>
                            u.CategoryAverages.ContainsKey(category)
                                ? u.CategoryAverages[category]
                                : 0
                        )
                        .ToList();
                }
            }

            // Apply pagination
            TotalPages = (int)Math.Ceiling(sortedSummaries.Count / (double)PageSize);
            PageNumber = Math.Clamp(PageNumber, 1, Math.Max(1, TotalPages));

            UserSummaries = sortedSummaries
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }
    }
}
