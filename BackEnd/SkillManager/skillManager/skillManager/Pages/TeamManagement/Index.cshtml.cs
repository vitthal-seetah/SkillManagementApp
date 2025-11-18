using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillManager.Application.DTOs.Project;
using SkillManager.Application.DTOs.Team;
using SkillManager.Application.DTOs.User;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Application.Mappers;
using SkillManager.Application.Models;

namespace SkillManager.Web.Pages.Teams;

[Authorize(Policy = "ManagerPolicy")]
public class ManageTeamsModel : PageModel
{
    private readonly ITeamService _teamService;
    private readonly IUserService _userService;
    private readonly IProjectService _projectService;
    private Domain.Entities.User? _currentUserEntity;

    public ManageTeamsModel(
        ITeamService teamService,
        IUserService userService,
        IProjectService projectService
    )
    {
        _teamService = teamService;
        _userService = userService;
        _projectService = projectService;
    }

    // Properties for the page
    public List<TeamDto> Teams { get; set; } = new();
    public List<TeamViewModel> TeamTable { get; set; } = new();
    public List<UserDto> Users { get; set; } = new();
    public List<ProjectDto> Projects { get; set; } = new();

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
                    // Get the project ID for this team
                    var teamProjectId = await _teamService.GetTeamProjectIdAsync(team.TeamId);

                    UpdateTeamDto = new UpdateTeamDto
                    {
                        TeamId = team.TeamId,
                        TeamName = team.TeamName,
                        TeamDescription = team.TeamDescription,
                        TeamLeadId = team.TeamLeadId ?? 0,
                        ProjectId = teamProjectId,
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
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                TempData["Error"] = "Unable to identify current user.";
                await LoadDataAsync();
                return Page();
            }

            // Validate that the project belongs to current user's project
            if (CreateTeamDto.ProjectId != currentUser.ProjectId)
            {
                TempData["Error"] = "You can only create teams in your own project.";
                await LoadDataAsync();
                return Page();
            }

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
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                TempData["Error"] = "Unable to identify current user.";
                await LoadDataAsync();
                return Page();
            }

            // Validate that the project belongs to current user's project
            if (UpdateTeamDto.ProjectId != currentUser.ProjectId)
            {
                TempData["Error"] = "You can only assign teams to your own project.";
                await LoadDataAsync();
                return Page();
            }

            UpdateTeamDto.TeamId = id;
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
        var currentUserEntity = await GetCurrentUserAsync();
        if (currentUserEntity == null)
        {
            Users = new List<UserDto>();
            Teams = new List<TeamDto>();
            TeamTable = new List<TeamViewModel>();
            Projects = new List<ProjectDto>();
            TempData["Error"] = "Could not load current user information.";
            return;
        }

        // Load users
        Users = (await _userService.GetAllAsync(currentUserEntity)).ToList();

        // Load teams
        Teams = (await _teamService.GetTeamsByProjectIdAsync(currentUserEntity.ProjectId)).ToList();

        // Load projects for dropdown (only current user's project)
        Projects = (await _projectService.GetAllProjectsAsync())
            .Where(p => p.ProjectId == currentUserEntity.ProjectId)
            .ToList();

        // Map TeamDto -> TeamViewModel with ProjectName fetched via GetByIdAsync
        TeamTable = new List<TeamViewModel>();
        foreach (var team in Teams)
        {
            string projectName = "Unknown";

            if (team.ProjectId > 0)
            {
                var project = await _projectService.GetByIdAsync(team.ProjectId);
                if (project != null)
                    projectName = project.ProjectName;
            }

            TeamTable.Add(
                new TeamViewModel
                {
                    TeamId = team.TeamId,
                    TeamName = team.TeamName,
                    TeamDescription = team.TeamDescription,
                    TeamLeadId = team.TeamLeadId,
                    MemberCount = team.MemberCount,
                    ProjectId = team.ProjectId,
                    ProjectName = projectName,
                }
            );
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
