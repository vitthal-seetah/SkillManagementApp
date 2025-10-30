using SkillManager.Application.Models;
using SkillManager.Infrastructure.DTOs.Skill;

namespace SkillManager.Application.Interfaces.Services;

public interface IUserSkillService
{
    Task<IEnumerable<UserSkillDto>> GetMySkillsAsync(int userId);
    Task AddSkillAsync(int userId, AddUserSkillDto dto);
    Task<bool> UpdateSkillAsync(int userId, UpdateUserSkillsDto dto);
    Task<IEnumerable<UserSkillDto>> GetAllUserSkillsAsync();
    Task<IEnumerable<UserSkillDto>> FilterBySkillAsync(string skillName);
    Task<IEnumerable<UserSkillsWithLevels>> GetUserSkillsByCategoryAsync(
        int categoryId,
        int userId
    );
    Task<CategoryNavigationViewModel> GetCategoryNavigationAsync(
        int? selectedCategoryId = null,
        int? userId = null
    );

    Task<List<CategoryTypeWithCategories>> GetCategoryTypesWithCategoriesAsync();
    Task DeleteUserSkillAsync(int userId);
}
