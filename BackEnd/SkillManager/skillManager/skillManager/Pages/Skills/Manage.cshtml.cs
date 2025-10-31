using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillManager.Application.DTOs.Category;
using SkillManager.Application.DTOs.Skill;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Application.Interfaces.Services;

namespace SkillManager.Web.Pages.Skills;

[Authorize(Policy = "ManagerPolicy")]
public class ManageModel : PageModel
{
    private readonly ISkillService _skillService;
    private readonly ICategoryService _categoryService;

    public ManageModel(ISkillService skillService, ICategoryService categoryService)
    {
        _skillService = skillService;
        _categoryService = categoryService;
    }

    // Properties bound to the page
    public List<SkillDto> Skills { get; set; } = new();
    public List<CategoryDto> Categories { get; set; } = new();
    public List<CategoryTypeDto> CategoryTypes { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? SelectedCategoryType { get; set; } = "All";

    [BindProperty(SupportsGet = true)]
    public string? SortBy { get; set; }

    // Pagination properties
    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public List<string> UserRoles { get; set; } = new();

    // ------------------------------------------------------------
    // OnGet - Load all skills with filters and sorting
    // ------------------------------------------------------------
    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            // Load categories using CategoryService
            var categories = await _categoryService.GetAllCategoriesAsync();
            Categories = categories.ToList();

            // Load category types using CategoryService
            var categoryTypes = await _categoryService.GetAllCategoryTypesAsync();
            CategoryTypes = categoryTypes.ToList();

            // Load all skills
            var allSkills = (await _skillService.GetAllSkillsAsync()).ToList();

            // Filter by category type
            if (!string.IsNullOrEmpty(SelectedCategoryType) && SelectedCategoryType != "All")
            {
                allSkills = allSkills
                    .Where(s =>
                        string.Equals(
                            s.CategoryTypeName,
                            SelectedCategoryType,
                            StringComparison.OrdinalIgnoreCase
                        )
                    )
                    .ToList();
            }

            // Sort results
            allSkills = SortBy switch
            {
                "CodeAsc" => allSkills.OrderBy(s => s.Code).ToList(),
                "CodeDesc" => allSkills.OrderByDescending(s => s.Code).ToList(),
                "NameAsc" => allSkills.OrderBy(s => s.Label).ToList(),
                "NameDesc" => allSkills.OrderByDescending(s => s.Label).ToList(),
                _ => allSkills,
            };

            // Apply pagination
            TotalCount = allSkills.Count;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            // Ensure PageNumber is within valid range
            PageNumber = Math.Max(1, Math.Min(PageNumber, TotalPages == 0 ? 1 : TotalPages));

            Skills = allSkills.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();

            UserRoles = new() { "Admin" };

            return Page();
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error loading skills: {ex.Message}";
            return Page();
        }
    }

    public async Task<JsonResult> OnGetSubCategories(int categoryId)
    {
        try
        {
            var subCategories = await _categoryService.GetSubCategoriesByCategoryAsync(categoryId);
            return new JsonResult(
                subCategories.Select(sc => new { subCategoryId = sc.SubCategoryId, name = sc.Name })
            );
        }
        catch (Exception ex)
        {
            return new JsonResult(new { error = ex.Message });
        }
    }

    // ------------------------------------------------------------
    // OnPostCreate - Create a new skill from the modal
    // ------------------------------------------------------------
    public async Task<IActionResult> OnPostCreateAsync([FromForm] CreateSkillDto dto)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Invalid input. Please fix the errors and try again.";
            return RedirectToPage();
        }

        try
        {
            bool result = await _skillService.CreateSkillAsync(dto);
            if (result)
                TempData["Success"] = $"Skill '{dto.Label}' created successfully.";
            else
                TempData["Error"] = "Failed to create skill.";
        }
        catch (ValidationException vex)
        {
            TempData["Error"] = vex.Message;
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Unexpected error: {ex.Message}";
        }

        return RedirectToPage();
    }

    // ------------------------------------------------------------
    // OnPostSave - Update an existing skill (inline edit)
    // ------------------------------------------------------------
    public async Task<IActionResult> OnPostSaveAsync(
        [FromForm] int id, // This now matches the form field name "id"
        [FromForm] UpdateSkillDto dto
    )
    {
        try
        {
            // Debugging: Check if ID is being received
            if (id == 0)
            {
                TempData["Error"] = "Skill ID is required for update.";
                return RedirectToPage();
            }

            var updatedSkill = await _skillService.UpdateSkillAsync(id, dto);
            TempData["Success"] = $"Skill '{updatedSkill.Label}' updated successfully.";
        }
        catch (ValidationException vex)
        {
            TempData["Error"] = vex.Message;
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error updating skill: {ex.Message}";
        }

        return RedirectToPage();
    }

    // ------------------------------------------------------------
    // OnPostDelete - Delete a skill
    // ------------------------------------------------------------
    public async Task<IActionResult> OnPostDeleteAsync([FromForm] int id)
    {
        try
        {
            if (id == 0)
            {
                TempData["Error"] = "Skill ID is required for deletion.";
                return RedirectToPage();
            }

            var result = await _skillService.DeleteSkillAsync(id);
            if (result)
                TempData["Success"] = "Skill deleted successfully.";
            else
                TempData["Error"] = "Failed to delete skill.";
        }
        catch (ValidationException vex)
        {
            TempData["Error"] = vex.Message;
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error deleting skill: {ex.Message}";
        }

        return RedirectToPage();
    }
}
