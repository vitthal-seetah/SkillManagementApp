using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillManager.Application.DTOs.Category;
using SkillManager.Application.DTOs.User;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Application.Models;
using SkillManager.Domain.Entities;

namespace SkillManager.Web.Pages.Users
{
    [Authorize(Policy = "EmployeePolicy")]
    public class UserSkillsModel : PageModel
    {
        private readonly IUserSkillService _userSkillService;
        private readonly IUserService _userService;

        public UserSkillsModel(IUserSkillService userSkillService, IUserService userService)
        {
            _userSkillService = userSkillService;
            _userService = userService;
        }

        public List<UserSkillsViewModel> UserSkills { get; set; } = new();
        public string CurrentUserName { get; set; } = "";
        public List<string> Roles { get; set; } = new();

        // --- Filtering & Sorting ---
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

        // --- Pagination ---
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 15;
        public int TotalPages { get; set; }

        public List<UserDto> AvailableUsers { get; set; } = new();
        public List<CategoryDto> AvailableCategories { get; set; } = new();
        public List<string> AvailableLevels { get; set; } =
            new() { "Connaissance", "Pratique", "Maitrise", "Expert" };

        public async Task OnGetAsync()
        {
            CurrentUserName = User.Identity?.Name ?? "Unknown User";
            Roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            // Get available users and categories for filters
            AvailableUsers = (await _userService.GetAllAsync()).ToList();
            AvailableCategories = (await _userSkillService.GetAllCategories()).ToList();

            // Get all user skills
            var allUserSkills = await _userSkillService.GetUserSkillsLevels();

            // --- Apply Filters ---
            if (!string.IsNullOrWhiteSpace(SelectedUser) && SelectedUser != "All")
            {
                if (int.TryParse(SelectedUser, out int userId))
                {
                    allUserSkills = allUserSkills.Where(us => us.UserId == userId).ToList();
                }
            }

            if (!string.IsNullOrWhiteSpace(SelectedCategory) && SelectedCategory != "All")
            {
                if (int.TryParse(SelectedCategory, out int categoryId))
                {
                    allUserSkills = allUserSkills.Where(us => us.CategoryId == categoryId).ToList();
                }
            }

            if (!string.IsNullOrWhiteSpace(SelectedSkill) && SelectedSkill != "All")
            {
                allUserSkills = allUserSkills
                    .Where(us =>
                        us.SkillLabel.Contains(SelectedSkill, StringComparison.OrdinalIgnoreCase)
                        || us.SkillCode.Contains(SelectedSkill, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(SelectedLevel) && SelectedLevel != "All")
            {
                allUserSkills = allUserSkills
                    .Where(us =>
                        us.LevelName.Equals(SelectedLevel, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
            }

            // --- Sorting ---
            allUserSkills = SortBy switch
            {
                "FirstNameAsc" => allUserSkills.OrderBy(us => us.FirstName).ToList(),
                "FirstNameDesc" => allUserSkills.OrderByDescending(us => us.FirstName).ToList(),
                "LastNameAsc" => allUserSkills.OrderBy(us => us.LastName).ToList(),
                "LastNameDesc" => allUserSkills.OrderByDescending(us => us.LastName).ToList(),
                "FullNameAsc" => allUserSkills
                    .OrderBy(us => $"{us.FirstName} {us.LastName}")
                    .ToList(),
                "FullNameDesc" => allUserSkills
                    .OrderByDescending(us => $"{us.FirstName} {us.LastName}")
                    .ToList(),
                "SkillLabelAsc" => allUserSkills.OrderBy(us => us.SkillLabel).ToList(),
                "SkillLabelDesc" => allUserSkills.OrderByDescending(us => us.SkillLabel).ToList(),
                "SkillCodeAsc" => allUserSkills.OrderBy(us => us.SkillCode).ToList(),
                "SkillCodeDesc" => allUserSkills.OrderByDescending(us => us.SkillCode).ToList(),
                "CategoryNameAsc" => allUserSkills.OrderBy(us => us.CategoryName).ToList(),
                "CategoryNameDesc" => allUserSkills
                    .OrderByDescending(us => us.CategoryName)
                    .ToList(),
                "LevelNameAsc" => allUserSkills.OrderBy(us => us.LevelName).ToList(),
                "LevelNameDesc" => allUserSkills.OrderByDescending(us => us.LevelName).ToList(),
                "LevelPointsAsc" => allUserSkills.OrderBy(us => us.LevelPoints).ToList(),
                "LevelPointsDesc" => allUserSkills.OrderByDescending(us => us.LevelPoints).ToList(),
                _ => allUserSkills
                    .OrderBy(us => us.FirstName)
                    .ThenBy(us => us.LastName)
                    .ThenBy(us => us.CategoryName)
                    .ThenBy(us => us.SkillLabel)
                    .ToList(),
            };

            // --- Pagination ---
            var totalSkills = allUserSkills.Count;
            TotalPages = (int)Math.Ceiling(totalSkills / (double)PageSize);
            PageNumber = Math.Clamp(PageNumber, 1, Math.Max(1, TotalPages));

            UserSkills = allUserSkills.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
        }
    }
}
