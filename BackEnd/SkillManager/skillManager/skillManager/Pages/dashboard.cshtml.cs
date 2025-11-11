using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Application.Models;
using SkillManager.Domain.Entities;

namespace SkillManager.Web.Pages
{
    [Authorize(Policy = "EmployeePolicy")]
    public class DashboardModel : PageModel
    {
        private readonly IUserSkillService _userSkillService;
        private readonly IUserService _userService;

        public DashboardModel(IUserSkillService userSkillService, IUserService userService)
        {
            _userSkillService = userSkillService;
            _userService = userService;
        }

        // --- Display properties ---
        public string DisplayUserName { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public bool IsViewingOwnDashboard { get; set; } = true;

        // --- Skill Data ---
        public List<UserSkillsViewModel> Skills { get; set; } = new();
        public Dictionary<string, int> SkillsByCategory { get; set; } = new();
        public Dictionary<string, int> SkillsByLevel { get; set; } = new();
        public Dictionary<string, double> AvgLevelByCategory { get; set; } = new();
        public Dictionary<string, int> SkillsOverTime { get; set; } = new();
        public List<UserSkillsViewModel> RecentSkills { get; set; } = new();

        public int TotalSkills { get; set; }
        public double AverageLevelPoints { get; set; }
        public string TopCategory { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; }

        public async Task OnGetAsync(int? userId)
        {
            var currentUser = await GetCurrentUserAsync();
            int targetUserId;

            // Determine which user's dashboard to display
            if (userId.HasValue)
            {
                // Viewing another user's dashboard (from ManagerDashboard navigation)
                targetUserId = userId.Value;
                IsViewingOwnDashboard = false;

                // Verify the target user exists and is in the same project
                var targetUserDto = await _userService.GetUserByIdAsync(targetUserId, currentUser);
                if (targetUserDto == null)
                {
                    // Fall back to current user if target user not found or not accessible
                    targetUserId = GetCurrentUserIdFromClaims();
                    IsViewingOwnDashboard = true;
                }
            }
            else
            {
                // Viewing own dashboard
                targetUserId = GetCurrentUserIdFromClaims();
                IsViewingOwnDashboard = true;
            }

            UserId = targetUserId.ToString();
            UserRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "No role";

            // Get user details using GetUserByIdAsync method
            var userDto = await _userService.GetUserByIdAsync(targetUserId, currentUser);
            if (userDto != null)
            {
                DisplayUserName = $"{userDto.FirstName} {userDto.LastName}";
            }
            else
            {
                DisplayUserName = "User Not Found";
                // If user not found, clear skills and return early
                Skills = new List<UserSkillsViewModel>();
                return;
            }

            // Get skills for the target user
            Skills = (await _userSkillService.GetMySkillsAsync(targetUserId)).ToList();
            if (!Skills.Any())
                return;

            // Populate summary
            TotalSkills = Skills.Count;
            AverageLevelPoints = Skills.Average(s => s.LevelPoints);
            var topCategoryGroup = Skills
                .GroupBy(s => s.CategoryName)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();
            TopCategory = topCategoryGroup?.Key ?? "No Category";
            LastUpdated = Skills.Max(s => s.UpdatedTime);

            // Prepare chart data
            SkillsByCategory = Skills
                .GroupBy(s => s.CategoryName)
                .ToDictionary(g => g.Key, g => g.Count());

            SkillsByLevel = Skills
                .GroupBy(s => s.LevelName)
                .ToDictionary(g => g.Key, g => g.Count());

            AvgLevelByCategory = Skills
                .GroupBy(s => s.CategoryName)
                .ToDictionary(g => g.Key, g => g.Average(x => x.LevelPoints));

            SkillsOverTime = Skills
                .GroupBy(s => s.UpdatedTime.ToString("yyyy-MM"))
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.Count());

            RecentSkills = Skills.OrderByDescending(s => s.UpdatedTime).Take(5).ToList();
        }

        private int GetCurrentUserIdFromClaims()
        {
            var uidClaim = User.Claims.FirstOrDefault(c => c.Type == "uid");
            if (uidClaim == null || !int.TryParse(uidClaim.Value, out int userId))
            {
                throw new InvalidOperationException("User ID claim not found or invalid.");
            }
            return userId;
        }

        private async Task<User?> GetCurrentUserAsync()
        {
            // Extract current user's identity from claims (similar to IndexModel)
            var fullName = User.Identity?.Name ?? "Unavailable";
            string domain = "";
            string eid = "";

            if (fullName.Contains('\\'))
            {
                var parts = fullName.Split('\\', 2);
                domain = parts[0];
                eid = parts[1];
            }
            else
            {
                eid = fullName;
            }

            // Get current user entity using the same pattern as IndexModel
            return await _userService.GetUserEntityByDomainAndEidAsync(domain, eid, null);
        }
    }
}
