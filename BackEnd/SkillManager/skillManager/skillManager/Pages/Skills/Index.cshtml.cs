using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.DTOs.Skill;

namespace SkillManager.Web.Pages.Skills
{
    [Authorize(Policy = "EmployeePolicy")]
    public class SkillIndexModel : PageModel
    {
        private readonly IUserSkillService _userSkillService;

        public SkillIndexModel(IUserSkillService userSkillService)
        {
            _userSkillService = userSkillService;
        }

        public List<UserSkillDto> MySkills { get; set; } = new();
        public string Username { get; set; } = "";
        public string UserId { get; set; } = "";
        public string DebugInfo { get; set; } = "";
        public string UserRole { get; set; } = " ";

        public async Task OnGetAsync()
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
                MySkills = (await _userSkillService.GetMySkillsAsync(userId)).ToList();
            }
            else
            {
                UserId = uidClaim?.Value ?? "No UID";
                MySkills = new List<UserSkillDto>();
            }
        }
    }
}
