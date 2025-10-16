using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillManager.Application.Abstractions.Identity;
using SkillManager.Application.Abstractions.Repository;

namespace SkillManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;

    public UsersController(IUserService userService, IUserRepository userRepository)
    {
        _userService = userService;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAll();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userService.GetUserById(id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost("update-identifiers")]
    public async Task<IActionResult> UpdateUserIdentifiers(
        string userId,
        string utCode,
        string employeeId
    )
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return NotFound("User not found");

        // manually set the values
        user.UTCode = utCode;
        user.EmployeeId = employeeId;

        // save the changes via repository
        await _userRepository.UpdateAsync(user);

        return Ok("User identifiers updated successfully");
    }
}
