using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Infrastructure.DTOs.Skill;

namespace SkillManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserSkillsController : ControllerBase
{
    private readonly IUserSkillService _userSkillService;

    public UserSkillsController(IUserSkillService userSkillService)
    {
        _userSkillService = userSkillService;
    }

    // -------------------------
    // USER: View their own skills
    // -------------------------

    [HttpGet("MySkills")]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> GetMySkills()
    {
        if (!TryGetCurrentUserId(out int userId))
            return Unauthorized(new { message = "Invalid user ID" });

        var skills = await _userSkillService.GetMySkillsAsync(userId);
        return Ok(skills);
    }

    // -------------------------
    // USER: Add a skill for themselves
    // -------------------------
    [Authorize(Policy = "EmployeePolicy")]
    [HttpPost("AddSkill")]
    public async Task<IActionResult> AddSkill([FromBody] AddUserSkillDto dto)
    {
        if (!TryGetCurrentUserId(out int userId))
            return Unauthorized(new { message = "Invalid user ID" });

        try
        {
            await _userSkillService.AddSkillAsync(userId, dto);
            return Ok(new { message = "Skill added successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // -------------------------
    // USER: Update a skill for themselves
    // -------------------------
    [Authorize(Policy = "EmployeePolicy")]
    [HttpPut("UpdateSkill")]
    public async Task<IActionResult> UpdateSkill([FromBody] UpdateUserSkillsDto dto)
    {
        if (!TryGetCurrentUserId(out int userId))
            return Unauthorized(new { message = "Invalid user ID" });

        try
        {
            await _userSkillService.UpdateSkillAsync(userId, dto);
            return Ok(new { message = "Skill updated successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // -------------------------
    // LEADER / ADMIN: View all user skills
    // -------------------------
    [Authorize(Policy = "TechLeadPolicy")]
    [HttpGet("AllSkills")]
    public async Task<IActionResult> GetAllSkills()
    {
        var skills = await _userSkillService.GetAllUserSkillsAsync();
        return Ok(skills);
    }

    // -------------------------
    // LEADER / ADMIN: Filter users by skill name
    // -------------------------
    [Authorize(Policy = "TechLeadPolicy")]
    [HttpGet("FilterBySkill")]
    public async Task<IActionResult> FilterBySkill([FromQuery] string skillName)
    {
        var skills = await _userSkillService.FilterBySkillAsync(skillName);
        return Ok(skills);
    }

    // -------------------------
    // ADMIN ONLY: Delete a user skill
    // -------------------------
    [Authorize(Policy = "ManagerPolicy")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSkill(int id)
    {
        try
        {
            await _userSkillService.DeleteUserSkillAsync(id);
            return Ok(new { message = "Skill deleted successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // -------------------------
    // Helper: Parse userId from claims
    // -------------------------
    private bool TryGetCurrentUserId(out int userId)
    {
        userId = 0;
        var claimValue = User.FindFirstValue("uid");
        if (string.IsNullOrEmpty(claimValue))
            return false;

        return int.TryParse(claimValue, out userId);
    }
}
