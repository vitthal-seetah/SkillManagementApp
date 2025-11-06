using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillManager.Application.DTOs.Level;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Application.Interfaces.Repositories.m;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Application.Models;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.DTOs.Skill;

namespace SkillManager.Web.Pages.Skills
{
    [Authorize(Policy = "EmployeePolicy")]
    public class SkillIndexModel : PageModel
    {
        private readonly IUserSkillService _userSkillService;
        private readonly ISkillService _skillService;
        private readonly ILevelService _levelService;
        public CategoryNavigationViewModel ViewModel { get; set; } = new();

        public SkillIndexModel(
            IUserSkillService userSkillService,
            ISkillService skillService,
            ILevelService levelService
        )
        {
            _userSkillService = userSkillService;
            _skillService = skillService;
            _levelService = levelService;
        }

        public List<UserSkillsWithLevels> MySkills { get; set; } = new();
        public List<LevelDto> AvailableLevels { get; set; } = new(); // Add this property

        public string Username { get; set; } = "";
        public string UserId { get; set; } = "";
        public string DebugInfo { get; set; } = "";
        public string UserRole { get; set; } = " ";

        public async Task OnGetAsync(int? categoryId)
        {
            Username = User.Identity?.Name ?? "Unknown User";

            // Convert all claims to JSON for easy viewing
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

                // Debug what we got
                Console.WriteLine($"=== VIEW MODEL DATA ===");
                Console.WriteLine($"SelectedCategoryId: {ViewModel.SelectedCategoryId}");
                Console.WriteLine($"UserSkills count: {ViewModel.UserSkills?.Count ?? 0}");
                if (ViewModel.UserSkills != null)
                {
                    foreach (var skill in ViewModel.UserSkills)
                    {
                        Console.WriteLine($"Skill: {skill.Code}, Level: {skill.LevelName}");
                    }
                }
            }
            else
            {
                UserId = uidClaim?.Value ?? "No UID";
                // Get ViewModel without user skills (category navigation only)
                ViewModel = await _userSkillService.GetCategoryNavigationAsync(categoryId);
            }

            // Load available levels for dropdown (if needed)
            //    AvailableLevels = (await _levelService.GetAllLevelsAsync()).ToList();
            AvailableLevels = (await _levelService.GetAllLevelsAsync()).ToList();
        }

        public async Task<IActionResult> OnPostUpdateSkillLevelAsync(
            int skillId,
            int levelId,
            int? categoryId
        )
        {
            try
            {
                var uidClaim = User.Claims.FirstOrDefault(c => c.Type == "uid");
                if (uidClaim != null && int.TryParse(uidClaim.Value, out int userId))
                {
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
                else
                {
                    TempData["ErrorMessage"] = "User not found.";
                }
                return RedirectToPage(new { categoryId = categoryId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating skill level: {ex.Message}";
                return RedirectToPage(new { categoryId = categoryId });
            }
        }
    }
}
