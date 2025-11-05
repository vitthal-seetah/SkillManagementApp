using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillManager.Application.DTOs.Category;
using SkillManager.Application.DTOs.SubCategory;

namespace SkillManager.Web.Pages.Skills;

[Authorize(Policy = "ManagerPolicy")]
public class ManageCategoryModel : PageModel
{
    private readonly ICategoryService _categoryService;

    public ManageCategoryModel(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // Properties for the page
    public List<CategoryDto> Categories { get; set; } = new();
    public List<CategoryTypeDto> CategoryTypes { get; set; } = new();
    public List<SubCategoryDto> SubCategories { get; set; } = new();

    // Bind properties for forms
    [BindProperty]
    public CreateCategoryDto CreateCategoryDto { get; set; } = new();

    [BindProperty]
    public UpdateCategoryDto UpdateCategoryDto { get; set; } = new();

    [BindProperty]
    public CreateCategoryTypeDto CreateCategoryTypeDto { get; set; } = new();

    [BindProperty]
    public UpdateCategoryTypeDto UpdateCategoryTypeDto { get; set; } = new();

    [BindProperty]
    public CreateSubCategoryDto CreateSubCategoryDto { get; set; } = new();

    [BindProperty]
    public UpdateSubCategoryDto UpdateSubCategoryDto { get; set; } = new();

    public int? EditCategoryId { get; set; }
    public int? EditSubCategoryId { get; set; }
    public int? EditCategoryTypeId { get; set; }

    public async Task<IActionResult> OnGetAsync(
        int? editCategoryId = null,
        int? editSubCategoryId = null,
        int? editCategoryTypeId = null
    )
    {
        try
        {
            EditCategoryId = editCategoryId;
            EditSubCategoryId = editSubCategoryId;
            EditCategoryTypeId = editCategoryTypeId;

            // Load all data
            await LoadDataAsync();

            // If editing a category, load its data
            if (editCategoryId.HasValue)
            {
                var category = await _categoryService.GetCategoryByIdAsync(editCategoryId.Value);
                UpdateCategoryDto = new UpdateCategoryDto
                {
                    Name = category.Name,
                    CategoryTypeId = category.CategoryTypeId,
                };
            }

            // If editing a subcategory, load its data
            if (editSubCategoryId.HasValue)
            {
                var subCategory = await _categoryService.GetSubCategoryByIdAsync(
                    editSubCategoryId.Value
                );
                UpdateSubCategoryDto = new UpdateSubCategoryDto
                {
                    Name = subCategory.Name,
                    CategoryId = subCategory.CategoryId,
                };
            }

            // If editing a category type, load its data
            if (editCategoryTypeId.HasValue)
            {
                var categoryType = await _categoryService.GetCategoryTypeByIdAsync(
                    editCategoryTypeId.Value
                );
                UpdateCategoryTypeDto = new UpdateCategoryTypeDto { Name = categoryType.Name };
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

    // Category CRUD Operations
    public async Task<IActionResult> OnPostCreateCategoryAsync()
    {
        try
        {
            await _categoryService.CreateCategoryAsync(CreateCategoryDto);
            TempData["Success"] = $"Category '{CreateCategoryDto.Name}' created successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error creating category: {ex.Message}";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateCategoryAsync(int id)
    {
        try
        {
            await _categoryService.UpdateCategoryAsync(id, UpdateCategoryDto);
            TempData["Success"] = "Category updated successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error updating category: {ex.Message}";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteCategoryAsync(int id)
    {
        try
        {
            await _categoryService.DeleteCategoryAsync(id);
            TempData["Success"] = "Category deleted successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error deleting category: {ex.Message}";
        }

        return RedirectToPage();
    }

    // Category Type Operations
    public async Task<IActionResult> OnPostCreateCategoryTypeAsync()
    {
        try
        {
            await _categoryService.CreateCategoryTypeAsync(CreateCategoryTypeDto);
            TempData["Success"] =
                $"Category Type '{CreateCategoryTypeDto.Name}' created successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error creating category type: {ex.Message}";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateCategoryTypeAsync(int id)
    {
        try
        {
            await _categoryService.UpdateCategoryTypeAsync(id, UpdateCategoryTypeDto);
            TempData["Success"] = "Category Type updated successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error updating category type: {ex.Message}";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteCategoryTypeAsync(int id)
    {
        try
        {
            await _categoryService.DeleteCategoryTypeAsync(id);
            TempData["Success"] = "Category Type deleted successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error deleting category type: {ex.Message}";
        }

        return RedirectToPage();
    }

    // SubCategory CRUD Operations
    public async Task<IActionResult> OnPostCreateSubCategoryAsync()
    {
        try
        {
            await _categoryService.CreateSubCategoryAsync(CreateSubCategoryDto);
            TempData["Success"] =
                $"SubCategory '{CreateSubCategoryDto.Name}' created successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error creating subcategory: {ex.Message}";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateSubCategoryAsync(int id)
    {
        try
        {
            await _categoryService.UpdateSubCategoryAsync(id, UpdateSubCategoryDto);
            TempData["Success"] = "SubCategory updated successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error updating subcategory: {ex.Message}";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteSubCategoryAsync(int id)
    {
        try
        {
            await _categoryService.DeleteSubCategoryAsync(id);
            TempData["Success"] = "SubCategory deleted successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error deleting subcategory: {ex.Message}";
        }

        return RedirectToPage();
    }

    // AJAX handler for getting subcategories by category
    public async Task<JsonResult> OnGetSubCategoriesByCategory(int categoryId)
    {
        try
        {
            var subCategories = await _categoryService.GetSubCategoriesByCategoryAsync(categoryId);
            return new JsonResult(
                subCategories.Select(sc => new
                {
                    subCategoryId = sc.SubCategoryId,
                    name = sc.Name,
                    categoryName = sc.CategoryName,
                    skillCount = sc.SkillCount,
                })
            );
        }
        catch (Exception ex)
        {
            return new JsonResult(new { error = ex.Message });
        }
    }

    private async Task LoadDataAsync()
    {
        Categories = (await _categoryService.GetAllCategoriesAsync()).ToList();
        CategoryTypes = (await _categoryService.GetAllCategoryTypesAsync()).ToList();
        // For subcategories, we'll load them on demand via AJAX to avoid loading all at once
    }
}
