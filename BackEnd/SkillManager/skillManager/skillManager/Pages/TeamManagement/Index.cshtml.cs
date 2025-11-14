using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillManager.Application.DTOs.Team;
using SkillManager.Application.DTOs.User;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Application.Mappers;

namespace SkillManager.Web.Pages.Teams;

[Authorize(Policy = "ManagerPolicy")]
public class ManageTeamsModel : PageModel
{
    private readonly ITeamService _teamService;
    private readonly IUserService _userService;
    private Domain.Entities.User? _currentUserEntity;

    public ManageTeamsModel(ITeamService teamService, IUserService userService)
    {
        _teamService = teamService;
        _userService = userService;
    }

    // Properties for the page
    public List<TeamDto> Teams { get; set; } = new();
    public List<UserDto> Users { get; set; } = new();

    // Bind properties for forms
    [BindProperty]
    public CreateTeamDto CreateTeamDto { get; set; } = new();

    [BindProperty]
    public UpdateTeamDto UpdateTeamDto { get; set; } = new();

    public int? EditTeamId { get; set; }

    public async Task<IActionResult> OnGetAsync(int? editTeamId = null)
    {
        try
        {
            EditTeamId = editTeamId;

            // Load all data
            await LoadDataAsync();

            // If editing a team, load its data
            if (editTeamId.HasValue)
            {
                var team = await _teamService.GetTeamByIdAsync(editTeamId.Value);
                if (team != null)
                {
                    UpdateTeamDto = new UpdateTeamDto
                    {
                        TeamName = team.TeamName,
                        TeamDescription = team.TeamDescription,
                        TeamLeadId = team.TeamLeadId ?? 0,
                    };
                }
            }

            return Page();
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error loading data: {ex.Message}";
            await LoadDataAsync();
            return Page();
        }
    }

    // Team CRUD Operations
    public async Task<IActionResult> OnPostCreateTeamAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadDataAsync();
            return Page();
        }

        try
        {
            await _teamService.CreateTeamAsync(CreateTeamDto);
            TempData["Success"] = $"Team '{CreateTeamDto.TeamName}' created successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error creating team: {ex.Message}";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateTeamAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            await LoadDataAsync();
            return Page();
        }

        try
        {
            await _teamService.UpdateTeamAsync(UpdateTeamDto);
            TempData["Success"] = "Team updated successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error updating team: {ex.Message}";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteTeamAsync(int id)
    {
        try
        {
            await _teamService.DeleteTeamAsync(id);
            TempData["Success"] = "Team deleted successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error deleting team: {ex.Message}";
        }

        return RedirectToPage();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            // Get current user entity using the same approach as User Management
            var currentUserEntity = await GetCurrentUserAsync();

            if (currentUserEntity != null)
            {
                // Pass the User entity (not DTO) to the service method
                Users = (await _userService.GetAllAsync(currentUserEntity)).ToList();
            }
            else
            {
                Users = new List<UserDto>();
                TempData["Error"] = "Could not load current user information.";
            }

            // Load teams
            var teams = await _teamService.GetAllTeamsWithProjectsAsync();
            Teams = TeamMapper.ToDto(teams).ToList();
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error loading data: {ex.Message}";
            Teams = new List<TeamDto>();
            Users = new List<UserDto>();
        }
    }

    private async Task<Domain.Entities.User?> GetCurrentUserAsync()
    {
        if (_currentUserEntity != null)
            return _currentUserEntity;

        try
        {
            // Use the same approach as User Management Index page
            var fullName = User.Identity?.Name ?? "Unavailable";
            string domain;
            string eid;

            if (fullName.Contains('\\'))
            {
                var parts = fullName.Split('\\', 2);
                domain = parts[0];
                eid = parts[1];
            }
            else
            {
                domain = "";
                eid = fullName;
            }

            // Use the service method that doesn't require currentUser parameter
            _currentUserEntity = await _userService.GetUserEntityByDomainAndEidAsync(
                domain,
                eid,
                null
            );
            return _currentUserEntity;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting current user: {ex.Message}");
            return null;
        }
    }
}
