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
        private readonly ICategoryService _categoryService; // Add this

        public DashboardModel(
            IUserSkillService userSkillService,
            IUserService userService,
            ICategoryService categoryService
        )
        {
            _userSkillService = userSkillService;
            _userService = userService;
            _categoryService = categoryService; // Add this
        }

        // --- Existing properties ---
        public string DisplayUserName { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public bool IsViewingOwnDashboard { get; set; } = true;
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

        // Add this new property for category mapping
        public Dictionary<string, int> CategoryNameToIdMap { get; set; } = new();

        public async Task OnGetAsync(int? userId)
        {
            var currentUser = await GetCurrentUserAsync();
            int targetUserId;

            // Determine which user's dashboard to display (your existing code)
            if (userId.HasValue)
            {
                targetUserId = userId.Value;
                IsViewingOwnDashboard = false;
                var targetUserDto = await _userService.GetUserByIdAsync(targetUserId, currentUser);
                if (targetUserDto == null)
                {
                    targetUserId = GetCurrentUserIdFromClaims();
                    IsViewingOwnDashboard = true;
                }
            }
            else
            {
                targetUserId = GetCurrentUserIdFromClaims();
                IsViewingOwnDashboard = true;
            }

            UserId = targetUserId.ToString();
            UserRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "No role";

            // Get user details
            var userDto = await _userService.GetUserByIdAsync(targetUserId, currentUser);
            if (userDto != null)
            {
                DisplayUserName = $"{userDto.FirstName} {userDto.LastName}";
            }
            else
            {
                DisplayUserName = "User Not Found";
                Skills = new List<UserSkillsViewModel>();
                return;
            }

            // Get skills for the target user
            Skills = (await _userSkillService.GetMySkillsAsync(targetUserId)).ToList();
            if (!Skills.Any())
                return;

            // Build category name to ID mapping
            var allCategories = await _categoryService.GetAllCategoriesAsync();
            CategoryNameToIdMap = allCategories.ToDictionary(c => c.Name, c => c.CategoryId);

            // Populate summary (your existing code)
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

            return await _userService.GetUserEntityByDomainAndEidAsync(domain, eid, null);
        }
    }
}
