using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Application.Models;

namespace SkillManager.Web.Pages
{
    [Authorize(Policy = "EmployeePolicy")]
    public class DashboardModel : PageModel
    {
        private readonly IUserSkillService _userSkillService;

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

        // Optional: for display
        public string Username { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;

        public DashboardModel(IUserSkillService userSkillService)
        {
            _userSkillService = userSkillService;
        }

        public async Task OnGetAsync()
        {
            // 1?? Get user info from claims
            Username = User.Identity?.Name ?? "Unknown User";
            UserRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "No role";

            var uidClaim = User.Claims.FirstOrDefault(c => c.Type == "uid");
            if (uidClaim == null || !int.TryParse(uidClaim.Value, out int userId))
            {
                throw new InvalidOperationException("User ID claim not found or invalid.");
            }
            UserId = userId.ToString();

            // 2?? Get skills for this user
            Skills = (await _userSkillService.GetMySkillsAsync(userId)).ToList();
            if (!Skills.Any())
                return;

            // 3?? Populate summary
            TotalSkills = Skills.Count;
            AverageLevelPoints = Skills.Average(s => s.LevelPoints);
            TopCategory = Skills
                .GroupBy(s => s.CategoryName)
                .OrderByDescending(g => g.Count())
                .First()
                .Key;
            LastUpdated = Skills.Max(s => s.UpdatedTime);

            // 4?? Prepare chart data
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
    }
}
