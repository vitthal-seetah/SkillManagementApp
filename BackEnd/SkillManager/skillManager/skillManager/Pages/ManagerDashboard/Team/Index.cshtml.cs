using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillManager.Application.DTOs.Team;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.DTOs.Skill;

namespace SkillManager.web.Pages.ManagerDashboard.Team
{
    [Authorize(Policy = "ManagerPolicy")]
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IUserSkillService _userSkillService;
        private readonly ITeamService _teamService;

        public IndexModel(
            IUserService userService,
            IUserSkillService userSkillService,
            ITeamService teamService
        )
        {
            _userService = userService;
            _userSkillService = userSkillService;
            _teamService = teamService;
        }

        // Properties
        public List<UserSummary> UserSummaries { get; set; } = new();
        public List<UserSkillDto> TeamMembers { get; set; } = new();
        public List<UserSkillDto> FilteredTeamMembers { get; set; } = new();

        // NEW: Dictionary to map UserId to TeamName
        public Dictionary<int, string> UserTeamMap { get; set; } = new();

        // NEW: No Team Members Count
        public int NoTeamMemberCount { get; set; }

        // Manager and Project Info
        public string ManagerName { get; set; } = "";
        public string ProjectName { get; set; } = "";

        // MULTI-TEAM SUPPORT
        public IEnumerable<TeamDto> AllTeams { get; set; }
        public string SelectedTeamId { get; set; } = "All";

        // Statistics
        public int TotalTeamSkills { get; set; }
        public double TeamAverageLevel { get; set; }
        public DateTime? LastTeamUpdate { get; set; }
        public int UniqueTeamMembers =>
            FilteredTeamMembers.Select(t => t.UserId).Distinct().Count();

        // Filtering & Sorting Properties
        [BindProperty(SupportsGet = true)]
        public string? SelectedUser { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SelectedCategory { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SelectedSkill { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SelectedLevel { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SortBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Handler { get; set; }

        // Available options for filters
        public List<string> AvailableUsers =>
            TeamMembers.Select(t => $"{t.UserId}-{t.FirstName} {t.LastName}").Distinct().ToList();
        public List<string> AvailableCategories =>
            TeamMembers.Select(t => t.CategoryName).Distinct().ToList();
        public List<string> AvailableSkills =>
            TeamMembers.Select(t => t.SkillName).Distinct().ToList();
        public List<string> AvailableLevels => new() { "Notion", "Pratique", "Maitrise", "Expert" };

        public async Task OnGetAsync(string selectedTeamId = "All")
        {
            // Get current manager info
            var currentUserEntity = await GetCurrentUserEntityAsync();
            if (currentUserEntity != null)
            {
                ManagerName = $"{currentUserEntity.FirstName} {currentUserEntity.LastName}";
                ProjectName = currentUserEntity.Project?.ProjectName ?? "Current Project";

                // GET ALL TEAMS IN MANAGER'S PROJECT
                if (currentUserEntity.ProjectId.HasValue)
                {
                    AllTeams = await _teamService.GetTeamsByProjectIdAsync(
                        currentUserEntity.ProjectId
                    );

                    // NEW: Get user-team mapping for the entire project
                    UserTeamMap = await _teamService.GetUserTeamMapByProjectIdAsync(
                        currentUserEntity.ProjectId
                    );

                    // NEW: Calculate no team member count
                    await CalculateNoTeamMemberCountAsync(
                        currentUserEntity.ProjectId,
                        currentUserEntity
                    );
                }

                // Set selected team
                SelectedTeamId = selectedTeamId;

                // Get team members based on selection
                if (SelectedTeamId == "All")
                {
                    // Get skills for ALL teams in the project
                    TeamMembers = await GetAllTeamsSkillsAsync(currentUserEntity.ProjectId);
                }
                else if (SelectedTeamId == "NoTeam")
                {
                    // NEW: Get skills for members with no team
                    TeamMembers = await GetNoTeamSkillsAsync(
                        currentUserEntity.ProjectId,
                        currentUserEntity
                    );
                }
                else
                {
                    // Get skills for specific team
                    if (int.TryParse(SelectedTeamId, out int teamId))
                    {
                        TeamMembers = (
                            await _userSkillService.GetAllUserSkillsByTeamAsync(GetCurrentUserId())
                        ).ToList();

                        // NEW: For single team, all users belong to the same team
                        var selectedTeam = AllTeams.FirstOrDefault(t => t.TeamId == teamId);
                        if (selectedTeam != null)
                        {
                            var userIds = TeamMembers.Select(t => t.UserId).Distinct();
                            foreach (var userId in userIds)
                            {
                                UserTeamMap[userId] = selectedTeam.TeamName;
                            }
                        }
                    }
                    else
                    {
                        TeamMembers = new List<UserSkillDto>();
                    }
                }

                // Apply filters
                ApplyFilters();

                // Apply sorting
                ApplySorting();

                // Calculate team statistics
                CalculateStatistics();
            }
        }

        // NEW: Method to get current user entity instead of DTO
        private async Task<Domain.Entities.User?> GetCurrentUserEntityAsync()
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId > 0)
            {
                // Assuming you have a method to get user entity by ID
                // If not, you'll need to add this to your IUserService
                return await _userService.GetUserEntityByIdAsync(currentUserId);
            }
            return null;
        }

        // NEW: Method to calculate no team member count - FIXED
        private async Task CalculateNoTeamMemberCountAsync(int? projectId, User currentUser)
        {
            if (!projectId.HasValue)
                return;

            // Use the correct method that takes User entity
            var allProjectUsers = await _userService.GetAllAsync(currentUser);
            var usersWithNoTeam = allProjectUsers.Where(u => !u.TeamId.HasValue).ToList();
            NoTeamMemberCount = usersWithNoTeam.Count;
        }

        // NEW: Method to get skills for members with no team - FIXED
        private async Task<List<UserSkillDto>> GetNoTeamSkillsAsync(
            int? projectId,
            User currentUser
        )
        {
            var noTeamSkills = new List<UserSkillDto>();

            if (projectId.HasValue)
            {
                // Use the correct method that takes User entity
                var allProjectUsers = await _userService.GetAllAsync(currentUser);
                var usersWithNoTeam = allProjectUsers.Where(u => !u.TeamId.HasValue).ToList();

                foreach (var user in usersWithNoTeam)
                {
                    var userSkills = await _userSkillService.GetUserSkillsByUserIdAsync(
                        user.UserId
                    );
                    noTeamSkills.AddRange(userSkills);

                    // NEW: Add to UserTeamMap for consistent display
                    UserTeamMap[user.UserId] = "No Team";
                }
            }

            return noTeamSkills;
        }

        // NEW: Method to get TeamName for a user
        public string GetTeamNameForUser(int userId)
        {
            return UserTeamMap.ContainsKey(userId) ? UserTeamMap[userId] : "No Team";
        }

        private async Task<List<UserSkillDto>> GetAllTeamsSkillsAsync(int? projectId)
        {
            var allSkills = new List<UserSkillDto>();

            if (projectId.HasValue && AllTeams.Any())
            {
                foreach (var team in AllTeams)
                {
                    var teamSkills = await _userSkillService.GetAllUserSkillsByTeamAsync(
                        team.TeamId
                    );
                    allSkills.AddRange(teamSkills);
                }
            }

            return allSkills;
        }

        public async Task<IActionResult> OnGetExportAsync()
        {
            Console.WriteLine("Excel export function launched for Team Skills");

            // Load the same data as in OnGetAsync - FIXED
            var currentUserEntity = await GetCurrentUserEntityAsync();
            if (currentUserEntity != null)
            {
                ManagerName = $"{currentUserEntity.FirstName} {currentUserEntity.LastName}";
                ProjectName = currentUserEntity.Project?.ProjectName ?? "Current Project";

                // Get teams data same as OnGetAsync
                if (currentUserEntity.ProjectId.HasValue)
                {
                    AllTeams = await _teamService.GetTeamsByProjectIdAsync(
                        currentUserEntity.ProjectId
                    );
                    UserTeamMap = await _teamService.GetUserTeamMapByProjectIdAsync(
                        currentUserEntity.ProjectId
                    );
                    await CalculateNoTeamMemberCountAsync(
                        currentUserEntity.ProjectId,
                        currentUserEntity
                    );
                }

                if (SelectedTeamId == "All")
                {
                    TeamMembers = await GetAllTeamsSkillsAsync(currentUserEntity.ProjectId);
                }
                else if (SelectedTeamId == "NoTeam")
                {
                    TeamMembers = await GetNoTeamSkillsAsync(
                        currentUserEntity.ProjectId,
                        currentUserEntity
                    );
                }
                else
                {
                    if (int.TryParse(SelectedTeamId, out int teamId))
                    {
                        TeamMembers = (
                            await _userSkillService.GetAllUserSkillsByTeamAsync(GetCurrentUserId())
                        ).ToList();
                        var selectedTeam = AllTeams.FirstOrDefault(t => t.TeamId == teamId);
                        if (selectedTeam != null)
                        {
                            var userIds = TeamMembers.Select(t => t.UserId).Distinct();
                            foreach (var userId in userIds)
                            {
                                UserTeamMap[userId] = selectedTeam.TeamName;
                            }
                        }
                    }
                }

                ApplyFilters();
                CalculateStatistics();
            }

            // Generate Excel - UPDATE to include Team Name and handle NoTeam case
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("TeamSkills");

            // Add title with team/project info
            string teamInfo = SelectedTeamId switch
            {
                "All" => $"All Teams in {ProjectName}",
                "NoTeam" => $"Members with No Team in {ProjectName}",
                _ =>
                    $"Team: {AllTeams.FirstOrDefault(t => t.TeamId.ToString() == SelectedTeamId)?.TeamName ?? SelectedTeamId}",
            };

            ws.Cell(1, 1).Value = $"Team Skills Report - {teamInfo} - {DateTime.Now:yyyy-MM-dd}";
            ws.Cell(1, 1).Style.Font.Bold = true;
            ws.Cell(1, 1).Style.Font.FontSize = 16;
            ws.Range(1, 1, 1, 13).Merge(); // Updated to 13 columns

            // Add filters info
            ws.Cell(2, 1).Value = $"Project: {ProjectName}";
            ws.Cell(3, 1).Value = $"Generated By: {ManagerName}";
            ws.Cell(4, 1).Value = $"Team: {teamInfo}";
            ws.Cell(5, 1).Value = $"User Filter: {SelectedUser ?? "All"}";
            ws.Cell(6, 1).Value = $"Category Filter: {SelectedCategory ?? "All"}";
            ws.Cell(7, 1).Value = $"Skill Filter: {SelectedSkill ?? "All"}";
            ws.Cell(8, 1).Value = $"Level Filter: {SelectedLevel ?? "All"}";

            // Headers starting from row 10
            int headerRow = 10;
            ws.Cell(headerRow, 1).Value = "User ID";
            ws.Cell(headerRow, 2).Value = "First Name";
            ws.Cell(headerRow, 3).Value = "Last Name";
            ws.Cell(headerRow, 4).Value = "Team"; // Team column
            ws.Cell(headerRow, 5).Value = "Skill ID";
            ws.Cell(headerRow, 6).Value = "Skill Code";
            ws.Cell(headerRow, 7).Value = "Skill Name";
            ws.Cell(headerRow, 8).Value = "Category";
            ws.Cell(headerRow, 9).Value = "Level";
            ws.Cell(headerRow, 10).Value = "Level Points";
            ws.Cell(headerRow, 11).Value = "Last Updated";
            ws.Cell(headerRow, 12).Value = "Days Since Update";

            // Style headers
            var headerRange = ws.Range(headerRow, 1, headerRow, 12);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Data rows
            int row = headerRow + 1;
            foreach (var skill in FilteredTeamMembers)
            {
                ws.Cell(row, 1).Value = skill.UserId;
                ws.Cell(row, 2).Value = skill.FirstName;
                ws.Cell(row, 3).Value = skill.LastName;
                ws.Cell(row, 4).Value = GetTeamNameForUser(skill.UserId); // Use the mapping
                ws.Cell(row, 5).Value = skill.SkillId;
                ws.Cell(row, 6).Value = skill.SkillCode;
                ws.Cell(row, 7).Value = skill.SkillName;
                ws.Cell(row, 8).Value = skill.CategoryName;
                ws.Cell(row, 9).Value = skill.LevelName;
                ws.Cell(row, 10).Value = skill.LevelId;
                ws.Cell(row, 11).Value = skill.UpdatedTime.ToString("yyyy-MM-dd HH:mm");
                ws.Cell(row, 12).Value = (int)(DateTime.Now - skill.UpdatedTime).TotalDays;
                row++;
            }

            // Add summary statistics
            int summaryRow = row + 2;
            ws.Cell(summaryRow, 1).Value = "Summary Statistics";
            ws.Cell(summaryRow, 1).Style.Font.Bold = true;
            ws.Cell(summaryRow, 1).Style.Font.FontSize = 14;

            ws.Cell(summaryRow + 1, 1).Value = "Total Team Members:";
            ws.Cell(summaryRow + 1, 2).Value = UniqueTeamMembers;

            ws.Cell(summaryRow + 2, 1).Value = "Total Teams:";
            ws.Cell(summaryRow + 2, 2).Value = SelectedTeamId switch
            {
                "All" => AllTeams.Count(),
                "NoTeam" => 0, // No teams for NoTeam selection
                _ => 1,
            };

            ws.Cell(summaryRow + 3, 1).Value = "Total Skills:";
            ws.Cell(summaryRow + 3, 2).Value = TotalTeamSkills;

            ws.Cell(summaryRow + 4, 1).Value = "Average Skill Level:";
            ws.Cell(summaryRow + 4, 2).Value = Math.Round(TeamAverageLevel, 2);

            ws.Cell(summaryRow + 5, 1).Value = "Last Team Update:";
            ws.Cell(summaryRow + 5, 2).Value =
                LastTeamUpdate?.ToString("yyyy-MM-dd HH:mm") ?? "N/A";

            // Format columns
            ws.Columns().AdjustToContents();

            // Create the file stream
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"TeamSkills_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }

        public async Task<IActionResult> OnPostUpdateSkillLevelAsync(
            int skillId,
            int levelId,
            int userId
        )
        {
            try
            {
                Console.WriteLine($"=== UPDATE SKILL DEBUG ===");
                Console.WriteLine($"UserId: {userId}, SkillId: {skillId}, LevelId: {levelId}");

                var success = await _userSkillService.UpdateSkillAsync(
                    userId,
                    new UpdateUserSkillsDto
                    {
                        SkillId = skillId,
                        LevelId = levelId,
                        UpdatedTime = DateTime.Now,
                    }
                );

                if (success)
                {
                    TempData["SuccessMessage"] = "Skill level updated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update skill level.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating skill: {ex.Message}");
                TempData["ErrorMessage"] = $"Error updating skill level: {ex.Message}";
            }

            // Preserve filter and sort parameters
            return RedirectToPage(
                new
                {
                    selectedTeamId = SelectedTeamId,
                    SelectedUser = SelectedUser,
                    SelectedCategory = SelectedCategory,
                    SelectedSkill = SelectedSkill,
                    SelectedLevel = SelectedLevel,
                    SortBy = SortBy,
                }
            );
        }

        private void ApplyFilters()
        {
            FilteredTeamMembers = TeamMembers;

            // Filter by User
            if (!string.IsNullOrEmpty(SelectedUser) && SelectedUser != "All")
            {
                if (int.TryParse(SelectedUser.Split('-')[0], out int userId))
                {
                    FilteredTeamMembers = FilteredTeamMembers
                        .Where(us => us.UserId == userId)
                        .ToList();
                }
            }

            // Filter by Category
            if (!string.IsNullOrEmpty(SelectedCategory) && SelectedCategory != "All")
            {
                FilteredTeamMembers = FilteredTeamMembers
                    .Where(us =>
                        us.CategoryName.Equals(SelectedCategory, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
            }

            // Filter by Skill
            if (!string.IsNullOrEmpty(SelectedSkill) && SelectedSkill != "All")
            {
                FilteredTeamMembers = FilteredTeamMembers
                    .Where(us =>
                        us.SkillName.Contains(SelectedSkill, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
            }

            // Filter by Level
            if (!string.IsNullOrEmpty(SelectedLevel) && SelectedLevel != "All")
            {
                FilteredTeamMembers = FilteredTeamMembers
                    .Where(us =>
                        us.LevelName.Equals(SelectedLevel, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
            }
        }

        private void ApplySorting()
        {
            FilteredTeamMembers = SortBy switch
            {
                "UserNameAsc" => FilteredTeamMembers
                    .OrderBy(us => us.FirstName)
                    .ThenBy(us => us.LastName)
                    .ToList(),
                "UserNameDesc" => FilteredTeamMembers
                    .OrderByDescending(us => us.FirstName)
                    .ThenByDescending(us => us.LastName)
                    .ToList(),
                "SkillNameAsc" => FilteredTeamMembers.OrderBy(us => us.SkillName).ToList(),
                "SkillNameDesc" => FilteredTeamMembers
                    .OrderByDescending(us => us.SkillName)
                    .ToList(),
                "CategoryAsc" => FilteredTeamMembers.OrderBy(us => us.CategoryName).ToList(),
                "CategoryDesc" => FilteredTeamMembers
                    .OrderByDescending(us => us.CategoryName)
                    .ToList(),
                "LevelAsc" => FilteredTeamMembers.OrderBy(us => us.LevelId).ToList(),
                "LevelDesc" => FilteredTeamMembers.OrderByDescending(us => us.LevelId).ToList(),
                "UpdatedAsc" => FilteredTeamMembers.OrderBy(us => us.UpdatedTime).ToList(),
                "UpdatedDesc" => FilteredTeamMembers
                    .OrderByDescending(us => us.UpdatedTime)
                    .ToList(),
                _ => FilteredTeamMembers
                    .OrderBy(us => us.FirstName)
                    .ThenBy(us => us.LastName)
                    .ThenBy(us => us.SkillName)
                    .ToList(),
            };
        }

        private void CalculateStatistics()
        {
            if (FilteredTeamMembers.Any())
            {
                var userStats = FilteredTeamMembers
                    .GroupBy(us => us.UserId)
                    .Select(g => new
                    {
                        UserId = g.Key,
                        TotalSkills = g.Count(),
                        AverageLevel = g.Average(us => us.LevelId),
                        LastUpdated = g.Max(us => us.UpdatedTime),
                    })
                    .ToList();

                TotalTeamSkills = FilteredTeamMembers.Count;
                TeamAverageLevel = userStats.Average(u => u.AverageLevel);
                LastTeamUpdate = userStats.Max(u => u.LastUpdated);
            }
            else
            {
                TotalTeamSkills = 0;
                TeamAverageLevel = 0;
                LastTeamUpdate = null;
            }
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
    }
}
