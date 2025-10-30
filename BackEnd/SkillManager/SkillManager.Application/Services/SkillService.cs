using FluentValidation;
using global::SkillManager.Application.DTOs.Skill;
using global::SkillManager.Application.Interfaces.Repositories;
using global::SkillManager.Infrastructure.Exceptions;
using SkillManager.Application.DTOs.Category;
using SkillManager.Application.Mappers;
using SkillManager.Application.Models;

namespace SkillManager.Application.Services
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IValidator<UpdateSkillDto> _updateSkillValidator;

        public SkillService(
            ISkillRepository skillRepository,
            ICategoryRepository categoryRepository,
            IValidator<UpdateSkillDto> updateSkillValidator
        )
        {
            _skillRepository = skillRepository;
            _categoryRepository = categoryRepository;
            _updateSkillValidator = updateSkillValidator;
        }

        public async Task<SkillDto> GetSkillByIdAsync(int skillId)
        {
            var skill = await _skillRepository.GetByIdAsync(skillId);
            if (skill == null)
            {
                throw new NotFoundException($"Skill with ID {skillId} not found.");
            }

            return skill.ToSkillDto();
        }

        public async Task<SkillDto> GetSkillByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ValidationException("Skill code cannot be empty.");
            }

            var skill = await _skillRepository.GetByCodeAsync(code);
            if (skill == null)
            {
                throw new NotFoundException($"Skill with code '{code}' not found.");
            }

            return skill.ToSkillDto();
        }

        public async Task<IEnumerable<SkillDto>> GetAllSkillsAsync()
        {
            var skills = await _skillRepository.GetAllAsync();
            return skills.Select(s => s.ToSkillDto());
        }

        public async Task<bool> CreateSkillAsync(CreateSkillDto createDto)
        {
            var existingSkill = await _skillRepository.GetByCodeAsync(createDto.Code);
            if (existingSkill != null)
            {
                throw new ValidationException(
                    $"Skill with code '{createDto.Code}' already exists."
                );
            }

            var created = await _skillRepository.AddAsync(createDto.ToSkill());
            if (!created)
            {
                throw new ApplicationException("Failed to create skill.");
            }

            // Reload the skill to get related entities
            return created;
        }

        public async Task<SkillDto> UpdateSkillAsync(int skillId, UpdateSkillDto updateDto)
        {
            var existingSkill = await _skillRepository.GetByIdAsync(skillId);
            if (existingSkill == null)
            {
                throw new NotFoundException($"Skill with ID {skillId} not found.");
            }

            // --- Run FluentValidation ---
            var validationResult = await _updateSkillValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }

            // --- Apply updates only for provided properties ---
            if (updateDto.CategoryId.HasValue)
                existingSkill.CategoryId = updateDto.CategoryId.Value;

            if (updateDto.SubCategoryId.HasValue)
                existingSkill.SubCategoryId = updateDto.SubCategoryId.Value;

            if (!string.IsNullOrWhiteSpace(updateDto.Code))
                existingSkill.Code = updateDto.Code.Trim();

            if (!string.IsNullOrWhiteSpace(updateDto.Label))
                existingSkill.Label = updateDto.Label.Trim();

            if (!string.IsNullOrWhiteSpace(updateDto.CriticalityLevel))
                existingSkill.CriticalityLevel = updateDto.CriticalityLevel;

            if (updateDto.ProjectRequiresSkill.HasValue)
                existingSkill.ProjectRequiresSkill = updateDto.ProjectRequiresSkill.Value;

            if (updateDto.RequiredLevel.HasValue)
                existingSkill.RequiredLevel = updateDto.RequiredLevel.Value;

            if (updateDto.FirstLevelDescription != null)
                existingSkill.FirstLevelDescription = updateDto.FirstLevelDescription;

            if (updateDto.SecondLevelDescription != null)
                existingSkill.SecondLevelDescription = updateDto.SecondLevelDescription;

            if (updateDto.ThirdLevelDescription != null)
                existingSkill.ThirdLevelDescription = updateDto.ThirdLevelDescription;

            if (updateDto.FourthLevelDescription != null)
                existingSkill.FourthLevelDescription = updateDto.FourthLevelDescription;

            var updated = await _skillRepository.UpdateAsync(existingSkill);
            if (!updated)
            {
                throw new ApplicationException("Failed to update skill.");
            }

            var updatedSkill = await _skillRepository.GetByIdAsync(skillId);
            return updatedSkill.ToSkillDto();
        }

        public async Task<bool> DeleteSkillAsync(int skillId)
        {
            var skill = await _skillRepository.GetByIdAsync(skillId);
            if (skill is null)
            {
                throw new NotFoundException($"Skill with ID {skillId} not found.");
            }

            return await _skillRepository.DeleteAsync(skill);
        }

        public async Task<IEnumerable<SkillDto>> GetSkillsByCategoryAsync(int categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category is null)
            {
                throw new ValidationException("category id does not exist");
            }

            var skills = await _skillRepository.GetByCategoryAsync(category);
            return skills.Select(s => s.ToSkillDto());
        }

        public async Task<IEnumerable<SkillDto>> GetSkillsBySubCategoryAsync(int subCategoryId)
        {
            var subCategory = await _categoryRepository.GetSubCategoryByIdAsync(subCategoryId);

            if (subCategory is null)
            {
                throw new ValidationException("subcategory id does not exists");
            }
            var skills = await _skillRepository.GetBySubCategoryAsync(subCategory);
            return skills.Select(s => s.ToSkillDto());
        }

        public async Task<IEnumerable<SkillDto>> GetCriticalSkillsAsync()
        {
            var skills = await _skillRepository.GetCriticalSkillsAsync();
            return skills.Select(s => s.ToSkillDto());
        }

        public async Task<IEnumerable<SkillDto>> GetProjectRequiredSkillsAsync()
        {
            var skills = await _skillRepository.GetProjectRequiredSkillsAsync();
            return skills.Select(s => s.ToSkillDto());
        }

        public async Task<IEnumerable<SkillDto>> GetSkillsByRequiredLevelAsync(int level)
        {
            if (level < 1 || level > 4)
            {
                throw new ValidationException("Level must be between 1 and 4.");
            }

            var skills = await _skillRepository.GetAllAsync();
            return skills.Where(s => s.RequiredLevel == level).Select(s => s.ToSkillDto());
        }

        public async Task<IEnumerable<SkillDto>> SearchSkillsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Length < 2)
            {
                throw new ValidationException("Search term must be at least 2 characters long.");
            }

            var skills = await _skillRepository.GetAllAsync();
            return skills
                .Where(s =>
                    s.Code.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                    || s.Label.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                )
                .Select(s => s.ToSkillDto());
        }
    }
}
