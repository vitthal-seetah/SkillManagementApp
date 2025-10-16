using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillManager.Application.Abstractions.Services;
using SkillManager.Application.DTOs;
using SkillManager.Application.DTOs.Skill;

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
    [Authorize(Roles = "User,Leader,Admin")]
    [HttpGet("MySkills")]
    public async Task<IActionResult> GetMySkills()
    {
        var userId = User.FindFirstValue("uid");
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { message = "Invalid user ID" });

        var skills = await _userSkillService.GetMySkillsAsync(userId);
        return Ok(skills);
    }

    // -------------------------
    // USER: Add a skill for themselves
    // -------------------------
    [Authorize(Roles = "User,Leader,Admin")]
    [HttpPost("AddSkill")]
    public async Task<IActionResult> AddSkill([FromBody] AddUserSkillDto dto)
    {
        var userId = User.FindFirstValue("uid");
        if (string.IsNullOrEmpty(userId))
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
    [Authorize(Roles = "User,Leader,Admin")]
    [HttpPut("UpdateSkill")]
    public async Task<IActionResult> UpdateSkill([FromBody] UpdateUserSkillsDto dto)
    {
        var userId = User.FindFirstValue("uid");
        if (string.IsNullOrEmpty(userId))
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
    [Authorize(Roles = "Leader,Admin")]
    [HttpGet("AllSkills")]
    public async Task<IActionResult> GetAllSkills()
    {
        var skills = await _userSkillService.GetAllUserSkillsAsync();
        return Ok(skills);
    }

    // -------------------------
    // LEADER / ADMIN: Filter users by skill name
    // -------------------------
    [Authorize(Roles = "Leader,Admin")]
    [HttpGet("FilterBySkill")]
    public async Task<IActionResult> FilterBySkill([FromQuery] string skillName)
    {
        var skills = await _userSkillService.FilterBySkillAsync(skillName);
        return Ok(skills);
    }

    // -------------------------
    // ADMIN ONLY: Delete a user skill
    // -------------------------
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
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
}
