using System.Security.Claims;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
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
        private readonly ICategoryService _categoryService;

        public UserSkillsModel(
            IUserSkillService userSkillService,
            IUserService userService,
            ICategoryService categoryService
        )
        {
            _userSkillService = userSkillService;
            _userService = userService;
            _categoryService = categoryService;
        }

        public List<UserSkillsViewModel> UserSkills { get; set; } = new();
        public string CurrentUserName { get; set; } = "";
        public List<string> Roles { get; set; } = new();

        // --- Filtering & Sorting ---
        [BindProperty(SupportsGet = true)]
        public string? SelectedUser { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? UpdatedTime { get; set; }

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
        public int TotalSkills { get; set; }
        public List<UserSkillsViewModel> AllUserSkills { get; set; }
        public string UserId { get; set; } = "";
        public string UserRole { get; set; } = " ";

        public List<UserDto> AvailableUsers { get; set; } = new();
        public List<CategoryDto> AvailableCategories { get; set; } = new();
        public List<string> AvailableLevels { get; set; } =
            new() { "Connaissance", "Pratique", "Maitrise", "Expert" };
        public CategoryNavigationViewModel ViewModel { get; set; } = new();

        public async Task OnGetAsync(int? categoryyId)
        {
            Console.WriteLine("on get normal called");
            CurrentUserName = User.Identity?.Name ?? "Unknown User";
            Roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            // FIXED: Get current user entity using the same method as other pages
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

            var currentUser = await _userService.GetUserEntityByDomainAndEidAsync(
                domain,
                eid,
                null
            );

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
                ViewModel = await _userSkillService.GetCategoryNavigationAsync(
                    categoryyId,
                    currentUser.UserId
                );
            }
            if (currentUser != null)
            {
                // Get available users and categories for filters
                AvailableUsers = (await _userService.GetAllAsync(currentUser)).ToList();
                Console.WriteLine($"AvailableUsers count: {AvailableUsers.Count}");

                // Debug: Log the users being loaded
                foreach (var user in AvailableUsers)
                {
                    Console.WriteLine(
                        $"User: {user.FirstName} {user.LastName} (ID: {user.UserId})"
                    );
                }
            }
            else
            {
                Console.WriteLine("Current user is null - cannot load AvailableUsers");
                AvailableUsers = new List<UserDto>();
            }

            AvailableCategories = (await _categoryService.GetAllCategoriesAsync()).ToList();

            // Load user skills based on current user's permissions
            if (currentUser != null)
            {
                AllUserSkills = await _userSkillService.GetUserSkillsLevels();
            }
            else
            {
                AllUserSkills = new List<UserSkillsViewModel>();
            }

            // --- Apply Filters ---
            // Your existing filtering code remains the same...
            if (!string.IsNullOrWhiteSpace(SelectedUser) && SelectedUser != "All")
            {
                if (int.TryParse(SelectedUser, out int UserId))
                {
                    AllUserSkills = AllUserSkills.Where(us => us.UserId == UserId).ToList();
                }
            }

            if (!string.IsNullOrWhiteSpace(SelectedCategory) && SelectedCategory != "All")
            {
                if (int.TryParse(SelectedCategory, out int categoryId))
                {
                    AllUserSkills = AllUserSkills.Where(us => us.CategoryId == categoryId).ToList();
                }
            }

            if (!string.IsNullOrWhiteSpace(SelectedSkill) && SelectedSkill != "All")
            {
                AllUserSkills = AllUserSkills
                    .Where(us =>
                        us.SkillLabel.Contains(SelectedSkill, StringComparison.OrdinalIgnoreCase)
                        || us.SkillCode.Contains(SelectedSkill, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(SelectedLevel) && SelectedLevel != "All")
            {
                AllUserSkills = AllUserSkills
                    .Where(us =>
                        us.LevelName.Equals(SelectedLevel, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
            }

            // --- Sorting ---
            // Your existing sorting code remains the same...
            AllUserSkills = SortBy switch
            {
                "FirstNameAsc" => AllUserSkills.OrderBy(us => us.FirstName).ToList(),
                "FirstNameDesc" => AllUserSkills.OrderByDescending(us => us.FirstName).ToList(),
                "LastNameAsc" => AllUserSkills.OrderBy(us => us.LastName).ToList(),
                "LastNameDesc" => AllUserSkills.OrderByDescending(us => us.LastName).ToList(),
                "FullNameAsc" => AllUserSkills
                    .OrderBy(us => $"{us.FirstName} {us.LastName}")
                    .ToList(),
                "FullNameDesc" => AllUserSkills
                    .OrderByDescending(us => $"{us.FirstName} {us.LastName}")
                    .ToList(),
                "SkillLabelAsc" => AllUserSkills.OrderBy(us => us.SkillLabel).ToList(),
                "SkillLabelDesc" => AllUserSkills.OrderByDescending(us => us.SkillLabel).ToList(),
                "SkillCodeAsc" => AllUserSkills.OrderBy(us => us.SkillCode).ToList(),
                "SkillCodeDesc" => AllUserSkills.OrderByDescending(us => us.SkillCode).ToList(),
                "CategoryNameAsc" => AllUserSkills.OrderBy(us => us.CategoryName).ToList(),
                "CategoryNameDesc" => AllUserSkills
                    .OrderByDescending(us => us.CategoryName)
                    .ToList(),
                "UpdatedTimeAsc" => AllUserSkills.OrderBy(us => us.UpdatedTime).ToList(),
                "UpdatedTimeDesc" => AllUserSkills.OrderByDescending(us => us.UpdatedTime).ToList(),

                "LevelNameAsc" => AllUserSkills.OrderBy(us => us.LevelName).ToList(),
                "LevelNameDesc" => AllUserSkills.OrderByDescending(us => us.LevelName).ToList(),
                "LevelPointsAsc" => AllUserSkills.OrderBy(us => us.LevelPoints).ToList(),
                "LevelPointsDesc" => AllUserSkills.OrderByDescending(us => us.LevelPoints).ToList(),
                _ => AllUserSkills
                    .OrderBy(us => us.FirstName)
                    .ThenBy(us => us.LastName)
                    .ThenBy(us => us.CategoryName)
                    .ThenBy(us => us.SkillLabel)
                    .ToList(),
            };

            // --- Pagination ---
            TotalSkills = AllUserSkills.Count;
            TotalPages = (int)Math.Ceiling(TotalSkills / (double)PageSize);
            PageNumber = Math.Clamp(PageNumber, 1, Math.Max(1, TotalPages));

            UserSkills = AllUserSkills.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
        }

        public async Task<IActionResult> OnGetExportAsync()
        {
            Console.WriteLine("excel function  launched");
            // 1?? Get all skil
            AllUserSkills = await _userSkillService.GetUserSkillsLevels();
            // 2?? Apply same filters as OnGetAsync
            if (
                !string.IsNullOrWhiteSpace(SelectedUser)
                && SelectedUser != "All"
                && int.TryParse(SelectedUser, out int userId)
            )
                AllUserSkills = AllUserSkills.Where(us => us.UserId == userId).ToList();

            if (
                !string.IsNullOrWhiteSpace(SelectedCategory)
                && SelectedCategory != "All"
                && int.TryParse(SelectedCategory, out int categoryId)
            )
                AllUserSkills = AllUserSkills.Where(us => us.CategoryId == categoryId).ToList();

            if (!string.IsNullOrWhiteSpace(SelectedSkill) && SelectedSkill != "All")
                AllUserSkills = AllUserSkills
                    .Where(us =>
                        us.SkillLabel.Contains(SelectedSkill, StringComparison.OrdinalIgnoreCase)
                        || us.SkillCode.Contains(SelectedSkill, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();

            if (!string.IsNullOrWhiteSpace(SelectedLevel) && SelectedLevel != "All")
                AllUserSkills = AllUserSkills
                    .Where(us =>
                        us.LevelName.Equals(SelectedLevel, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();

            // 3?? Generate Excel
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("UserSkills");

            ws.Cell(1, 1).Value = "UserId";
            ws.Cell(1, 2).Value = "FirstName";
            ws.Cell(1, 3).Value = "LastName";
            ws.Cell(1, 4).Value = "SkillId";
            ws.Cell(1, 5).Value = "SkillCode";
            ws.Cell(1, 6).Value = "SkillLabel";
            ws.Cell(1, 7).Value = "CategoryId";
            ws.Cell(1, 8).Value = "CategoryName";
            ws.Cell(1, 9).Value = "LevelId";
            ws.Cell(1, 10).Value = "LevelName";
            ws.Cell(1, 11).Value = "LevelPoints";
            ws.Cell(1, 12).Value = "UpdatedTime";

            int row = 2;
            foreach (var s in AllUserSkills)
            {
                ws.Cell(row, 1).Value = s.UserId;
                ws.Cell(row, 2).Value = s.FirstName;
                ws.Cell(row, 3).Value = s.LastName;
                ws.Cell(row, 4).Value = s.SkillId;
                ws.Cell(row, 5).Value = s.SkillCode;
                ws.Cell(row, 6).Value = s.SkillLabel;
                ws.Cell(row, 7).Value = s.CategoryId;
                ws.Cell(row, 8).Value = s.CategoryName;
                ws.Cell(row, 9).Value = s.LevelId;
                ws.Cell(row, 10).Value = s.LevelName;
                ws.Cell(row, 11).Value = s.LevelPoints;
                ws.Cell(row, 12).Value = s.UpdatedTime.ToString("yyyy-MM-dd");
                row++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"UserSkills_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }
    }
}
