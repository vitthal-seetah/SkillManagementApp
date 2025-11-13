using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillManager.Application.DTOs.Category;
using SkillManager.Application.DTOs.Skill;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Application.Models;
using SkillManager.Domain.Entities;

namespace SkillManager.Web.Pages.SkillGap
{
    [Authorize(Policy = "EmployeePolicy")]
    public class IndexModel : PageModel
    {
        private readonly IUserSkillService _userSkillService;
        private readonly ICategoryService _categoryService;

        public IndexModel(IUserSkillService userSkillService, ICategoryService categoryService)
        {
            _userSkillService = userSkillService;
            _categoryService = categoryService;
        }

        public List<SkillGapDto> SkillGaps { get; set; } = new();
        public List<SkillGapDto> FilteredSkillGaps { get; set; } = new();
        public List<CategoryGapDto> GapsByCategory { get; set; } = new();
        public List<CategoryDto> AvailableCategories { get; set; } = new();

        // Filter properties
        [BindProperty(SupportsGet = true)]
        public int? SelectedCategoryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SelectedGapStatus { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SortBy { get; set; }

        // Summary statistics
        public int TotalSkills { get; set; }
        public int MeetingTarget { get; set; }
        public int NeedsImprovement { get; set; }
        public int CriticalGaps { get; set; }
        public int ProjectRequiredSkills { get; set; }
        public int ProjectRequiredGaps { get; set; }
        public int FilteredSkillCount { get; set; }
        public CategoryNavigationViewModel ViewModel { get; set; } = new();
        public string UserRole { get; set; } = " ";
        public string UserId { get; set; } = "";

        public async Task OnGetAsync(int? categoryId, string? gapStatus, string? sortBy)
        {
            SelectedCategoryId = categoryId;
            SelectedGapStatus = gapStatus;
            SortBy = sortBy ?? "skill-name";
            var claimsJson = System.Text.Json.JsonSerializer.Serialize(
                User.Claims.Select(c => new { Type = c.Type, Value = c.Value }),
                new System.Text.Json.JsonSerializerOptions { WriteIndented = true }
            );

            Console.WriteLine("=== CLAIMS AS JSON ===");
            Console.WriteLine(claimsJson);

            // Simple UID extraction
            UserRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "No role";

            var uidClaim = User.Claims.FirstOrDefault(c => c.Type == "uid");
            if (uidClaim != null && int.TryParse(uidClaim.Value, out int userId))
            {
                UserId = userId.ToString();

                // Get the ViewModel with user skills
                ViewModel = await _userSkillService.GetCategoryNavigationAsync(categoryId, userId);

                // Get available categories for filter dropdown
                AvailableCategories = (await _categoryService.GetAllCategoriesAsync()).ToList();

                // Get skill gaps from service
                SkillGaps = await _userSkillService.GetSkillGapsAsync(userId);

                // Apply filters
                FilteredSkillGaps = ApplyFilters(SkillGaps);

                // Apply sorting
                FilteredSkillGaps = ApplySorting(FilteredSkillGaps);

                // Calculate summary statistics for all skills
                TotalSkills = SkillGaps.Count;
                MeetingTarget = SkillGaps.Count(g => g.GapSize <= 0);
                NeedsImprovement = SkillGaps.Count(g => g.GapSize == 1);
                CriticalGaps = SkillGaps.Count(g => g.GapSize >= 2);
                ProjectRequiredSkills = SkillGaps.Count(g => g.ProjectRequiresSkill);
                ProjectRequiredGaps = SkillGaps.Count(g => g.ProjectRequiresSkill && g.GapSize > 0);
                FilteredSkillCount = FilteredSkillGaps.Count;

                // Get gaps by category from service or calculate locally
                GapsByCategory = await _userSkillService.GetSkillGapsByCategoryAsync(userId);
            }
        }

        private List<SkillGapDto> ApplyFilters(List<SkillGapDto> skills)
        {
            var filtered = skills.AsEnumerable();

            // Category filter - using the same pattern as UserSkills
            if (SelectedCategoryId.HasValue && SelectedCategoryId.Value > 0)
            {
                filtered = filtered.Where(s => s.CategoryId == SelectedCategoryId.Value);
            }

            // Gap status filter
            if (!string.IsNullOrEmpty(SelectedGapStatus))
            {
                filtered = SelectedGapStatus switch
                {
                    "on-track" => filtered.Where(s => s.GapSize <= 0),
                    "needs-improvement" => filtered.Where(s => s.GapSize == 1),
                    "critical" => filtered.Where(s => s.GapSize >= 2),
                    "project-required" => filtered.Where(s => s.ProjectRequiresSkill),
                    _ => filtered,
                };
            }

            return filtered.ToList();
        }

        private List<SkillGapDto> ApplySorting(List<SkillGapDto> skills)
        {
            return SortBy switch
            {
                "skill-name-desc" => skills.OrderByDescending(s => s.SkillName).ToList(),
                "gap-size" => skills.OrderBy(s => s.GapSize).ThenBy(s => s.SkillName).ToList(),
                "gap-size-desc" => skills
                    .OrderByDescending(s => s.GapSize)
                    .ThenBy(s => s.SkillName)
                    .ToList(),
                "priority" => skills
                    .OrderBy(s => s.GapSize) // Low to high: negative first, then 0, then positive
                    .ThenByDescending(s => s.ProjectRequiresSkill)
                    .ThenBy(s => s.SkillName)
                    .ToList(),

                "priority-desc" => skills
                    .OrderByDescending(s => s.GapSize) // High to low: positive first, then 0, then negative
                    .ThenByDescending(s => s.ProjectRequiresSkill)
                    .ThenBy(s => s.SkillName)
                    .ToList(),
                "project-required" => skills
                    .OrderByDescending(s => s.ProjectRequiresSkill)
                    .ThenBy(s => s.GapSize)
                    .ThenBy(s => s.SkillName)
                    .ToList(),
                "category" => skills.OrderBy(s => s.CategoryName).ThenBy(s => s.SkillName).ToList(),
                "category-desc" => skills
                    .OrderByDescending(s => s.CategoryName)
                    .ThenBy(s => s.SkillName)
                    .ToList(),
                _ => skills.OrderBy(s => s.SkillName).ToList(), // Default: skill-name
            };
        }
    }
}
