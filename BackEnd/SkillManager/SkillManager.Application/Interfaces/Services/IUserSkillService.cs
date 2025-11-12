using SkillManager.Application.DTOs.Category;
using SkillManager.Application.DTOs.Skill;
using SkillManager.Application.Models;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.DTOs.Skill;

namespace SkillManager.Application.Interfaces.Services;

public interface IUserSkillService
{
    Task<IEnumerable<UserSkillsViewModel>> GetMySkillsAsync(int userId);
    Task AddSkillAsync(int userId, AddUserSkillDto dto);
    Task<bool> UpdateSkillAsync(int userId, UpdateUserSkillsDto dto);
    Task<IEnumerable<UserSkillDto>> GetAllUserSkillsAsync();
    Task<IEnumerable<UserSkillDto>> GetAllUserSkillsByTeamAsync(int userId);

    Task<IEnumerable<UserSkillDto>> FilterBySkillAsync(string skillName);
    Task<IEnumerable<UserSkillsWithLevels>> GetUserSkillsByCategoryAsync(
        int categoryId,
        int userId
    );
    Task<CategoryNavigationViewModel> GetCategoryNavigationAsync(
        int? selectedCategoryId = null,
        int? userId = null
    );
    Task<List<CategoryDto>> GetAllCategories();

    Task<List<UserSkillsViewModel>> GetUserSkillsLevels();

    Task<List<SkillGapDto>> GetSkillGapsAsync(int userId);
    Task<List<CategoryGapDto>> GetSkillGapsByCategoryAsync(int userId);
    Task<List<CategoryTypeWithCategories>> GetCategoryTypesWithCategoriesAsync();
    Task DeleteUserSkillAsync(int userId);
    Task<DateTime?> GetLastUpdatedTimeAsync(int userId);
    Task<DateTime?> GetLastUpdatedTimeForSkillAsync(int userId, int skillId);
    Task<Dictionary<int, DateTime>> GetLastUpdatedTimesByCategoryAsync(int userId, int categoryId);
    Task<IEnumerable<UserSkillDto>> GetUserSkillsByUserIdAsync(int userId);
}
