using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillManager.Application.Abstractions.Identity;
using SkillManager.Domain.Entities;
using SkillManager.Domain.Entities.Enums;
using SkillManager.Infrastructure.Abstractions.Identity;
using SkillManager.Infrastructure.Abstractions.Repository;

namespace SkillManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;

    public UsersController(IUserService userService, IUserRepository userRepository)
    {
        _userService = userService;
        _userRepository = userRepository;
    }

    // Anyone with access (Admin, Manager, User) can list users
    [HttpGet]
    [Authorize(Roles = "Admin,Manager,Employee,Tech Lead,SME")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAll();
        return Ok(users);
    }

    // ✅ Test endpoint for Windows Authentication
    [HttpGet("current")]
    [AllowAnonymous] // or [Authorize] if you want only authenticated users to access
    public IActionResult GetCurrentUser()
    {
        var userFull = HttpContext.User.Identity?.Name; // e.g. DOMAIN\vitthal.seetah

        if (string.IsNullOrEmpty(userFull))
            return Ok("No user detected (Windows Auth might not be enabled).");

        var username = userFull.Contains('\\') ? userFull.Split('\\').Last() : userFull;

        return Ok(new { FullName = userFull, UserName = username });
    }

    [HttpGet("debug-claims")]
    public IActionResult DebugClaims()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

        return Ok(claims);
    }

    [Authorize(Policy = "ManagerPolicy")]
    [HttpGet("check-roles")]
    public IActionResult CheckRoles()
    {
        var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

        return Ok(new { Username = User.Identity?.Name, Roles = roles });
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("admin-only")]
    public IActionResult AdminEndpoint()
    {
        var username = User.Identity?.Name;
        return Ok($"Hello {username}, you are authorized as Admin!");
    }

    // Anyone with access can get a user by ID
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Manager,Employee,Tech Lead,SME")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userService.GetUserById(id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    // Admin: update ID-related fields (UtCode, RefId)
    [HttpPost("update-identifiers")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUserIdentifiers(
        string userId,
        string utCode,
        string refId
    )
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return NotFound("User not found");

        user.UTCode = utCode;
        user.RefId = refId;

        await _userRepository.UpdateAsync(user);
        return Ok("User identifiers updated successfully");
    }

    // Manager: update other fields (FirstName, LastName, Status, DeliveryType)
    [HttpPost("update-details")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> UpdateUserDetails(
        string userId,
        string firstName,
        string lastName,
        string status,
        string deliveryType
    )
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return NotFound("User not found");

        user.FirstName = firstName;
        user.LastName = lastName;

        if (Enum.TryParse(status, true, out UserStatus userStatus))
            user.Status = userStatus;

        if (Enum.TryParse(deliveryType, true, out DeliveryType userDeliveryType))
            user.DeliveryType = userDeliveryType;

        await _userRepository.UpdateAsync(user);
        return Ok("User details updated successfully");
    }
}
