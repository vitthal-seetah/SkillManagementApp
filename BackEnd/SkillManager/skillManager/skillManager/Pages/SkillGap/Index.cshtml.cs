using System.Security.Claims;
using global::SkillManager.Application.DTOs.Category;
using global::SkillManager.Application.DTOs.Skill;
using global::SkillManager.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillManager.Application.DTOs.Level;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Application.Interfaces.Repositories.m;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Application.Models;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.DTOs.Skill;

namespace SkillManager.Web.Pages.SkillGap
{
    [Authorize(Policy = "EmployeePolicy")]
    public class IndexModel : PageModel
    {
        private readonly IUserSkillService _userSkillService;

        public IndexModel(IUserSkillService userSkillService)
        {
            _userSkillService = userSkillService;
        }

        public List<SkillGapDto> SkillGaps { get; set; } = new();
        public List<CategoryGapDto> GapsByCategory { get; set; } = new();
        public int TotalSkills { get; set; }
        public int MeetingTarget { get; set; }
        public int NeedsImprovement { get; set; }
        public int CriticalGaps { get; set; }
        public int ProjectRequiredSkills { get; set; }
        public int ProjectRequiredGaps { get; set; }

        public async Task OnGetAsync()
        {
            var uidClaim = User.Claims.FirstOrDefault(c => c.Type == "uid");
            if (uidClaim != null && int.TryParse(uidClaim.Value, out int userId))
            {
                // Get skill gaps from service
                SkillGaps = await _userSkillService.GetSkillGapsAsync(userId);

                // Calculate summary statistics
                TotalSkills = SkillGaps.Count;
                MeetingTarget = SkillGaps.Count(g => g.GapSize <= 0);
                NeedsImprovement = SkillGaps.Count(g => g.GapSize == 1);
                CriticalGaps = SkillGaps.Count(g => g.GapSize >= 2);
                ProjectRequiredSkills = SkillGaps.Count(g => g.ProjectRequiresSkill);
                ProjectRequiredGaps = SkillGaps.Count(g => g.ProjectRequiresSkill && g.GapSize > 0);

                // Get gaps by category from service or calculate locally
                GapsByCategory = await _userSkillService.GetSkillGapsByCategoryAsync(userId);
            }
        }
    }
}
