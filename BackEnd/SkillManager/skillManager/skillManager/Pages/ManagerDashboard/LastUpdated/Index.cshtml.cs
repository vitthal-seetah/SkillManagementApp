using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillManager.Application.DTOs;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.DTOs.Skill;

namespace SkillManager.web.Pages.ManagerDashboard.LastUpdated;

[Authorize(Policy = "ManagerPolicy")]
public class IndexModel : PageModel
{
    private readonly IUserService _userService;
    private readonly IUserSkillService _userSkillService;

    public IndexModel(IUserService userService, IUserSkillService userSkillService)
    {
        _userService = userService;
        _userSkillService = userSkillService;
    }

    // Properties
    public List<UserSummary> UserSummaries { get; set; } = new();

    public List<TeamMemberLastUpdatedDto> TeamMembers { get; set; } = new();
    public string ManagerName { get; set; } = "";
    public string TeamName { get; set; } = "";

    // Statistics
    public int SkillsUpdatedToday { get; set; }
    public int SkillsUpdatedThisWeek { get; set; }
    public int SkillsUpdatedThisMonth { get; set; }
    public int RecentlyActiveMembers { get; set; }
    public double AverageDaysSinceUpdate { get; set; }
    public int InactiveMembers { get; set; }
    public int ActiveMembers { get; set; }
    public int WarningMembers { get; set; }

    // Filtering & Sorting Properties
    [BindProperty(SupportsGet = true)]
    public string? ActivityPeriod { get; set; } = "All";

    [BindProperty(SupportsGet = true)]
    public string? SortBy { get; set; } = "RecentFirst";

    public async Task OnGetAsync()
    {
        // Get current team lead info
        var currentUserId = GetCurrentUserId();
        if (currentUserId > 0)
        {
            var currentUser = await _userService.GetByIdAsync(currentUserId);
            ManagerName = $"{currentUser.FirstName} {currentUser.LastName}";
            TeamName = currentUser.TeamName ?? "My Team";

            // Get all team skills
            var teamSkills = (
                await _userSkillService.GetAllUserSkillsByTeamAsync(currentUserId)
            ).ToList();

            // Get last updated skill for each team member
            TeamMembers = GetLastUpdatedSkills(teamSkills);

            // Apply filters
            ApplyFilters();

            // Apply sorting
            ApplySorting();

            // Calculate statistics
            CalculateStatistics();
        }
    }

    private List<TeamMemberLastUpdatedDto> GetLastUpdatedSkills(List<UserSkillDto> teamSkills)
    {
        return teamSkills
            .GroupBy(skill => new
            {
                skill.UserId,
                skill.FirstName,
                skill.LastName,
            })
            .Select(g => new TeamMemberLastUpdatedDto
            {
                UserId = g.Key.UserId,
                FirstName = g.Key.FirstName,
                LastName = g.Key.LastName,
                SkillName = g.OrderByDescending(s => s.UpdatedTime).First().SkillName,
                SkillCode = g.OrderByDescending(s => s.UpdatedTime).First().SkillCode,
                CategoryName = g.OrderByDescending(s => s.UpdatedTime).First().CategoryName,
                LevelName = g.OrderByDescending(s => s.UpdatedTime).First().LevelName,
                LevelId = g.OrderByDescending(s => s.UpdatedTime).First().LevelId,
                LastUpdated = g.Max(s => s.UpdatedTime),
                DaysSinceUpdate = (int)(DateTime.Now - g.Max(s => s.UpdatedTime)).TotalDays,
            })
            .ToList();
    }

    private void ApplyFilters()
    {
        var now = DateTime.Now;

        TeamMembers = ActivityPeriod switch
        {
            "Today" => TeamMembers.Where(m => m.LastUpdated.Date == now.Date).ToList(),
            "ThisWeek" => TeamMembers.Where(m => m.LastUpdated >= now.AddDays(-7)).ToList(),
            "ThisMonth" => TeamMembers.Where(m => m.LastUpdated >= now.AddDays(-30)).ToList(),
            "Last30Days" => TeamMembers.Where(m => m.LastUpdated >= now.AddDays(-30)).ToList(),
            "Last90Days" => TeamMembers.Where(m => m.LastUpdated >= now.AddDays(-90)).ToList(),
            _ => TeamMembers,
        };
    }

    private void ApplySorting()
    {
        TeamMembers = SortBy switch
        {
            "RecentFirst" => TeamMembers.OrderByDescending(m => m.LastUpdated).ToList(),
            "RecentLast" => TeamMembers.OrderBy(m => m.LastUpdated).ToList(),
            "UserNameAsc" => TeamMembers.OrderBy(m => m.FirstName).ThenBy(m => m.LastName).ToList(),
            "UserNameDesc" => TeamMembers
                .OrderByDescending(m => m.FirstName)
                .ThenByDescending(m => m.LastName)
                .ToList(),
            "DaysSinceAsc" => TeamMembers.OrderBy(m => m.DaysSinceUpdate).ToList(),
            "DaysSinceDesc" => TeamMembers.OrderByDescending(m => m.DaysSinceUpdate).ToList(),
            _ => TeamMembers.OrderByDescending(m => m.LastUpdated).ToList(),
        };
    }

    private void CalculateStatistics()
    {
        var now = DateTime.Now;

        SkillsUpdatedToday = TeamMembers.Count(m => m.LastUpdated.Date == now.Date);
        SkillsUpdatedThisWeek = TeamMembers.Count(m => m.LastUpdated >= now.AddDays(-7));
        SkillsUpdatedThisMonth = TeamMembers.Count(m => m.LastUpdated >= now.AddDays(-30));
        RecentlyActiveMembers = TeamMembers.Count(m => m.DaysSinceUpdate < 7);
        AverageDaysSinceUpdate = TeamMembers.Any()
            ? TeamMembers.Average(m => m.DaysSinceUpdate)
            : 0;

        ActiveMembers = TeamMembers.Count(m => m.DaysSinceUpdate < 7);
        WarningMembers = TeamMembers.Count(m => m.DaysSinceUpdate >= 7 && m.DaysSinceUpdate < 30);
        InactiveMembers = TeamMembers.Count(m => m.DaysSinceUpdate >= 30);
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

    // Add these public helper methods that are called from the Razor view
    public string GetLevelBadgeClass(string levelName)
    {
        return levelName?.ToLower() switch
        {
            "expert" => "level-expert",
            "maitrise" => "level-maitrise",
            "pratique" => "level-pratique",
            "notion" => "level-notion",
            _ => "level-notion",
        };
    }

    public string GetLevelProgressBarClass(string levelName)
    {
        return levelName?.ToLower() switch
        {
            "expert" => "bg-expert",
            "maitrise" => "bg-maitrise",
            "pratique" => "bg-pratique",
            "notion" => "bg-notion",
            _ => "bg-notion",
        };
    }

    public string GetDaysBadgeClass(int daysSinceUpdate)
    {
        return daysSinceUpdate switch
        {
            < 7 => "bg-success",
            < 30 => "bg-warning",
            _ => "bg-danger",
        };
    }

    public string GetActivityBadgeClass(int daysSinceUpdate)
    {
        return daysSinceUpdate switch
        {
            < 7 => "bg-success",
            < 30 => "bg-warning",
            _ => "bg-danger",
        };
    }

    public string GetActivityStatus(int daysSinceUpdate)
    {
        return daysSinceUpdate switch
        {
            < 1 => "Very Active",
            < 7 => "Active",
            < 30 => "Needs Update",
            _ => "Inactive",
        };
    }

    public string GetRecencyTextClass(int daysSinceUpdate)
    {
        return daysSinceUpdate switch
        {
            < 1 => "text-success",
            < 7 => "text-primary",
            < 30 => "text-warning",
            _ => "text-danger",
        };
    }
}

public class TeamMemberLastUpdatedDto
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string SkillName { get; set; } = "";
    public string SkillCode { get; set; } = "";
    public string CategoryName { get; set; } = "";
    public string LevelName { get; set; } = "";
    public int LevelId { get; set; }
    public DateTime LastUpdated { get; set; }
    public int DaysSinceUpdate { get; set; }
}
