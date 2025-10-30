using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillManager.Application.Interfaces.Services;

namespace SkillManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // Anyone with access can list users
    [HttpGet]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    // Get current authenticated user
    [HttpGet("current")]
    [AllowAnonymous] // Or [Authorize(Policy = "EmployeePolicy")] if only authenticated users
    public IActionResult GetCurrentUser()
    {
        var userFull = HttpContext.User.Identity?.Name;

        if (string.IsNullOrEmpty(userFull))
            return Ok("No user detected (Windows Auth might not be enabled).");

        var username = userFull.Contains('\\') ? userFull.Split('\\').Last() : userFull;

        return Ok(new { FullName = userFull, UserName = username });
    }

    [HttpGet("debug-claims")]
    [Authorize(Policy = "ManagerPolicy")]
    public IActionResult DebugClaims()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        return Ok(claims);
    }

    [HttpGet("check-roles")]
    [Authorize(Policy = "ManagerPolicy")]
    public IActionResult CheckRoles()
    {
        var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        return Ok(new { Username = User.Identity?.Name, Roles = roles });
    }

    [HttpGet("admin-only")]
    [Authorize(Policy = "AdminPolicy")]
    public IActionResult AdminEndpoint()
    {
        var username = User.Identity?.Name;
        return Ok($"Hello {username}, you are authorized as Admin!");
    }

    // Get user by ID
    [HttpGet("{id}")]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }
}

//    // Admin: update ID-related fields (UTCode, RefId)
//    [HttpPost("update-identifiers")]
//    [Authorize(Policy = "AdminPolicy")]
//    public async Task<IActionResult> UpdateUserIdentifiers(int userId, string utCode, string refId)
//    {
//        var result = await _userService.UpdateUserIdentifiersAsync(userId, utCode, refId);
//        if (!result)
//            return NotFound("User not found");

//        return Ok("User identifiers updated successfully");
//    }

//    // Manager: update personal info, status, delivery type
//    [HttpPost("update-details")]
//    [Authorize(Policy = "ManagerPolicy")]
//    public async Task<IActionResult> UpdateUserDetails(
//        int userId,
//        string firstName,
//        string lastName,
//        string? status = null,
//        string? deliveryType = null
//    )
//    {
//        var result = await _userService.UpdateUserDetailsAsync(
//            userId,
//            firstName,
//            lastName,
//            status,
//            deliveryType
//        );
//        if (!result)
//            return NotFound("User not found");

//        return Ok("User details updated successfully");
//    }
//}
