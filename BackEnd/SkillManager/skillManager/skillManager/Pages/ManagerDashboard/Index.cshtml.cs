using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Domain.Entities;

namespace SkillManager.Web.Pages
{
    [Authorize(Policy = "ManagerPolicy")]
    public class ManagerDashboardModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IUserSkillService _userSkillService;
        private readonly ICategoryService _categoryService;
        private User? _currentUserEntity;

        public ManagerDashboardModel(
            IUserService userService,
            IUserSkillService userSkillService,
            ICategoryService categoryService
        )
        {
            _userService = userService;
            _userSkillService = userSkillService;
            _categoryService = categoryService;
        }

        public List<UserSummary> UserSummaries { get; set; } = new();
        public string FullName { get; set; } = "";
        public string Domain { get; set; } = "";
        public string Eid { get; set; } = "";
        public List<string> Roles { get; set; } = new();
        public Dictionary<string, string> CategoryColors { get; set; } = new();
        public Dictionary<string, string> RoleColors { get; set; } = new(); // NEW: Fixed role colors

        // Sorting property
        [BindProperty(SupportsGet = true)]
        public string? SortBy { get; set; }

        // New properties for header information
        public string ManagerTeamName { get; set; } = "";
        public int TeammateCount { get; set; }
        public string ProjectName { get; set; } = "";

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
            _currentUserEntity = await _userService.GetUserEntityByDomainAndEidAsync(
                Domain,
                Eid,
                null
            );
            return _currentUserEntity;
        }

        public async Task OnGetAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                TempData["Error"] = "User not found or access denied.";
                return;
            }

            // Get all users in the current user's project
            var allUsers = await _userService.GetAllAsync(currentUser);

            // Set header information
            ProjectName = currentUser.Project?.ProjectName ?? "No Project";
            ManagerTeamName = currentUser.Team?.TeamName ?? "No Team";

            // Count teammates (users in the same team, excluding the manager)
            if (currentUser.TeamId.HasValue)
            {
                TeammateCount = allUsers.Count(u =>
                    u.TeamId == currentUser.TeamId && u.UserId != currentUser.UserId
                );
            }
            else
            {
                // If no team, count all other users in the project
                TeammateCount = allUsers.Count(u => u.UserId != currentUser.UserId);
            }

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
            UserSummaries = new List<UserSummary>();

            foreach (var user in allUsers)
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

                UserSummaries.Add(summary);
            }

            // NEW: Set up fixed colors for roles
            var distinctRoles = UserSummaries
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

            // Apply Sorting
            UserSummaries = SortBy switch
            {
                "UserAsc" => UserSummaries
                    .OrderBy(u => u.FirstName)
                    .ThenBy(u => u.LastName)
                    .ToList(),
                "UserDesc" => UserSummaries
                    .OrderByDescending(u => u.FirstName)
                    .ThenByDescending(u => u.LastName)
                    .ToList(),
                "UtCodeAsc" => UserSummaries.OrderBy(u => u.UtCode).ToList(),
                "UtCodeDesc" => UserSummaries.OrderByDescending(u => u.UtCode).ToList(),
                "RoleAsc" => UserSummaries.OrderBy(u => u.RoleName).ToList(),
                "RoleDesc" => UserSummaries.OrderByDescending(u => u.RoleName).ToList(),
                "TeamAsc" => UserSummaries.OrderBy(u => u.TeamName ?? "").ToList(),
                "TeamDesc" => UserSummaries.OrderByDescending(u => u.TeamName ?? "").ToList(),
                "TotalSkillsAsc" => UserSummaries.OrderBy(u => u.TotalSkills).ToList(),
                "TotalSkillsDesc" => UserSummaries.OrderByDescending(u => u.TotalSkills).ToList(),
                "OverallAverageAsc" => UserSummaries.OrderBy(u => u.OverallAverage).ToList(),
                "OverallAverageDesc" => UserSummaries
                    .OrderByDescending(u => u.OverallAverage)
                    .ToList(),
                "LastUpdatedAsc" => UserSummaries
                    .OrderBy(u => u.LastUpdated ?? DateTime.MinValue)
                    .ToList(),
                "LastUpdatedDesc" => UserSummaries
                    .OrderByDescending(u => u.LastUpdated ?? DateTime.MinValue)
                    .ToList(),
                _ => UserSummaries.OrderByDescending(u => u.OverallAverage).ToList(), // Default sort
            };

            // Handle category sorting
            if (!string.IsNullOrEmpty(SortBy) && CategoryColors.Keys.Any(c => SortBy.StartsWith(c)))
            {
                var category = CategoryColors.Keys.First(c => SortBy.StartsWith(c));
                if (SortBy.EndsWith("Asc"))
                {
                    UserSummaries = UserSummaries
                        .OrderBy(u =>
                            u.CategoryAverages.ContainsKey(category)
                                ? u.CategoryAverages[category]
                                : 0
                        )
                        .ToList();
                }
                else if (SortBy.EndsWith("Desc"))
                {
                    UserSummaries = UserSummaries
                        .OrderByDescending(u =>
                            u.CategoryAverages.ContainsKey(category)
                                ? u.CategoryAverages[category]
                                : 0
                        )
                        .ToList();
                }
            }
        }
    }
}
