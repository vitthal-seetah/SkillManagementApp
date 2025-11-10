using SkillManager.Application.DTOs.Category;
using SkillManager.Application.DTOs.Skill;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Application.Mappers;
using SkillManager.Application.Models;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.DTOs.Skill;

namespace SkillManager.Application.Services
{
    public class UserSkillService : IUserSkillService
    {
        private readonly IUserSkillRepository _userSkillRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;

        public UserSkillService(
            IUserSkillRepository userSkillRepository,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository
        )
        {
            _userSkillRepository = userSkillRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<UserSkillsViewModel>> GetMySkillsAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var skills = await _userSkillRepository.GetUserSkillsAsync(userId);
            return skills.Select(skill => skill.ToUserSkillsViewModel());
        }

        public async Task<IEnumerable<UserSkillsWithLevels>> GetUserSkillsByCategoryAsync(
            int categoryId,
            int userId
        )
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                throw new InvalidOperationException("category not found");
            }

            // get all skills by category

            var skills = await _userSkillRepository.GetSkillsByCategory(category, user);
            if (skills == null)
            {
                throw new InvalidOperationException("category not found");
            }
            return skills.Select(s => s.ToUserSkillsWithLevels());
        }

        public async Task<List<SkillGapDto>> GetSkillGapsAsync(int userId)
        {
            return await _userSkillRepository.GetSkillGapsAsync(userId);
        }

        public async Task<List<CategoryGapDto>> GetSkillGapsByCategoryAsync(int userId)
        {
            return await _userSkillRepository.GetSkillGapsByCategoryAsync(userId);
        }

        public async Task<List<UserSkillsViewModel>> GetUserSkillsLevels()
        {
            var userSkills = await _userSkillRepository.GetAllUserSkillsLevels();
            return userSkills.Select(us => us.ToUserSkillsViewModel()).ToList();
        }

        public async Task AddSkillAsync(int userId, AddUserSkillDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var existing = await _userSkillRepository.GetByCompositeKeyAsync(userId, dto.SkillId);
            if (existing != null)
                throw new InvalidOperationException("Skill already exists for this user.");

            var userSkill = new UserSkill
            {
                UserId = userId,
                SkillId = dto.SkillId,
                LevelId = dto.LevelId,
                UpdatedTime = dto.UpdatedTime,
            };

            await _userSkillRepository.AddAsync(userSkill);
            await _userSkillRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateSkillAsync(int userId, UpdateUserSkillsDto dto)
        {
            var userSkill = await _userSkillRepository.GetByCompositeKeyAsync(userId, dto.SkillId);
            if (userSkill == null)
                throw new InvalidOperationException("Skill not found for this user.");

            userSkill.LevelId = dto.LevelId;
            userSkill.UpdatedTime = dto.UpdatedTime;

            await _userSkillRepository.UpdateAsync(userSkill);
            await _userSkillRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserSkillDto>> GetAllUserSkillsAsync()
        {
            var skills = await _userSkillRepository.GetAllAsync();
            return skills.Select(MapToDto);
        }

        public async Task<IEnumerable<UserSkillDto>> FilterBySkillAsync(string skillName)
        {
            var skills = await _userSkillRepository.FilterBySkillAsync(skillName);
            return skills.Select(MapToDto);
        }

        public async Task DeleteUserSkillAsync(int userId)
        {
            var userSkills = await _userSkillRepository.GetUserSkillsAsync(userId);
            if (!userSkills.Any())
                throw new InvalidOperationException("No skills found for this user.");

            foreach (var skill in userSkills)
            {
                await _userSkillRepository.DeleteAsync(userId, skill.SkillId);
            }

            await _userSkillRepository.SaveChangesAsync();
        }

        public async Task<List<CategoryDto>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => c.ToCategoryDto()).ToList();
        }

        public async Task<CategoryNavigationViewModel> GetCategoryNavigationAsync(
            int? selectedCategoryId = null,
            int? userId = null
        )
        {
            // First create the viewModel without the async data
            var viewModel = new CategoryNavigationViewModel
            {
                CategoryTypes = await GetCategoryTypesWithCategoriesAsync(),
                SelectedCategoryId = selectedCategoryId,
            };

            // Then load user skills separately if needed
            if (selectedCategoryId.HasValue && userId.HasValue)
            {
                try
                {
                    // Move the async call outside the property assignment
                    var userSkills = await GetUserSkillsByCategoryAsync(
                        selectedCategoryId.Value,
                        userId.Value
                    );
                    viewModel.UserSkills = userSkills.ToList();
                }
                catch (Exception)
                {
                    viewModel.UserSkills = new List<UserSkillsWithLevels>();
                }
            }
            else
            {
                viewModel.UserSkills = new List<UserSkillsWithLevels>();
            }

            return viewModel;
        }

        public async Task<List<CategoryTypeWithCategories>> GetCategoryTypesWithCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            // Group categories by their CategoryType
            var result = categories
                .GroupBy(c => new { c.CategoryTypeId, c.CategoryType.Name })
                .Select(g => new CategoryTypeWithCategories
                {
                    CategoryTypeId = g.Key.CategoryTypeId,
                    Name = g.Key.Name,
                    Categories = g.Select(c => new CategoryDto
                        {
                            CategoryId = c.CategoryId,
                            Name = c.Name,
                            CategoryTypeId = c.CategoryTypeId,
                        })
                        .ToList(),
                })
                .OrderBy(ct => ct.Name)
                .ToList();

            return result;
        }

        public async Task<DateTime?> GetLastUpdatedTimeAsync(int userId)
        {
            var userSkills = await _userSkillRepository.GetUserSkillsAsync(userId);

            if (!userSkills.Any())
                return null;

            return userSkills.Max(us => us.UpdatedTime);
        }

        public async Task<DateTime?> GetLastUpdatedTimeForSkillAsync(int userId, int skillId)
        {
            var userSkill = await _userSkillRepository.GetByCompositeKeyAsync(userId, skillId);
            return userSkill?.UpdatedTime;
        }

        public async Task<Dictionary<int, DateTime>> GetLastUpdatedTimesByCategoryAsync(
            int userId,
            int categoryId
        )
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
                return new Dictionary<int, DateTime>();

            var skills = await _userSkillRepository.GetSkillsByCategory(
                category,
                await _userRepository.GetByIdAsync(userId)
            );
            return skills?.ToDictionary(s => s.SkillId, s => s.UpdatedTime)
                ?? new Dictionary<int, DateTime>();
        }

        private static UserSkillDto MapToDto(UserSkill us)
        {
            return new UserSkillDto
            {
                UserId = us.UserId,
                SkillId = us.SkillId,
                SkillName = us.Skill?.Label ?? "", // <-- Map to SkillName
                SkillCode = us.Skill?.Code ?? "",
                CategoryName = us.Skill?.Category?.Name ?? "",
                CategoryType = us.Skill?.Category?.CategoryType?.Name.ToString() ?? "",
                LevelId = us.LevelId,
                LevelName = us.Level?.Name ?? "",
                UpdatedTime = us.UpdatedTime,
            };
        }
    }
}
