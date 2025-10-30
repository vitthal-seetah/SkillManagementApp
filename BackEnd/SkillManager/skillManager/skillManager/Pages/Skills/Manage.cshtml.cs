using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillManager.Application.DTOs.Category;
using SkillManager.Application.DTOs.Skill;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Domain.Entities.Enums;

namespace SkillManager.Web.Pages.Skills
{
    public class ManageModel : PageModel
    {
        private readonly ISkillService _skillService;
        private readonly ICategoryRepository _categoryRepository;

        public ManageModel(ISkillService skillService, ICategoryRepository categoryRepository)
        {
            _skillService = skillService;
            _categoryRepository = categoryRepository;
        }

        //Properties bound to the page
        public List<SkillDto> Skills { get; set; } = new();
        public List<CategoryDto> Categories { get; set; } = new();
        public List<CategoryTypeDto> CategoryTypes { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SelectedCategoryType { get; set; } = "All";

        [BindProperty(SupportsGet = true)]
        public string? SortBy { get; set; }

        public List<string> UserRoles { get; set; } = new();

        // ------------------------------------------------------------
        //OnGet - Load all skills with filters and sorting
        // ------------------------------------------------------------
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Load categories
                var categories = await _categoryRepository.GetAllAsync();
                Categories = categories
                    .Select(c => new CategoryDto
                    {
                        CategoryId = c.CategoryId,
                        Name = c.Name,
                        CategoryTypeId = c.CategoryTypeId,
                        CategoryTypeName = c.CategoryType?.Name ?? "",
                    })
                    .ToList();

                // Load category types
                var categoryTypes = await _categoryRepository.GetAllCategoryTypesAsync();
                CategoryTypes = categoryTypes
                    .Select(ct => new CategoryTypeDto
                    {
                        CategoryTypeId = ct.CategoryTypeId,
                        Name = ct.Name,
                    })
                    .ToList();

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

                Skills = allSkills;
                UserRoles = new() { "Admin" };

                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading skills: {ex.Message}";
                return Page();
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
        //OnPostSave - Update an existing skill (inline edit)
        // ------------------------------------------------------------
        public async Task<IActionResult> OnPostSaveAsync(
            [FromForm] int id,
            [FromForm] UpdateSkillDto dto
        )
        {
            try
            {
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
    }
}
