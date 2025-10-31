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

            return created;
        }

        public async Task<SkillDto> UpdateSkillAsync(int skillId, UpdateSkillDto updateDto)
        {
            var existingSkill = await _skillRepository.GetByIdAsync(skillId);
            if (existingSkill == null)
            {
                throw new NotFoundException($"Skill with ID {skillId} not found.");
            }

            // Create a copy of the updateDto with only the fields that are actually being updated
            var updateDtoForValidation = new UpdateSkillDto();
            var hasChanges = false;

            // Only validate fields that are being updated
            if (
                updateDto.CategoryId.HasValue
                && updateDto.CategoryId.Value != existingSkill.CategoryId
            )
            {
                updateDtoForValidation.CategoryId = updateDto.CategoryId;
                hasChanges = true;
            }

            if (
                updateDto.SubCategoryId.HasValue
                && updateDto.SubCategoryId.Value != existingSkill.SubCategoryId
            )
            {
                updateDtoForValidation.SubCategoryId = updateDto.SubCategoryId;
                hasChanges = true;
            }

            if (
                !string.IsNullOrWhiteSpace(updateDto.Code)
                && updateDto.Code.Trim() != existingSkill.Code
            )
            {
                updateDtoForValidation.Code = updateDto.Code;
                hasChanges = true;
            }

            if (
                !string.IsNullOrWhiteSpace(updateDto.Label)
                && updateDto.Label.Trim() != existingSkill.Label
            )
            {
                updateDtoForValidation.Label = updateDto.Label;
                hasChanges = true;
            }

            if (
                !string.IsNullOrWhiteSpace(updateDto.CriticalityLevel)
                && updateDto.CriticalityLevel != existingSkill.CriticalityLevel
            )
            {
                updateDtoForValidation.CriticalityLevel = updateDto.CriticalityLevel;
                hasChanges = true;
            }

            if (
                updateDto.ProjectRequiresSkill.HasValue
                && updateDto.ProjectRequiresSkill.Value != existingSkill.ProjectRequiresSkill
            )
            {
                updateDtoForValidation.ProjectRequiresSkill = updateDto.ProjectRequiresSkill;
                hasChanges = true;
            }

            if (
                updateDto.RequiredLevel.HasValue
                && updateDto.RequiredLevel.Value != existingSkill.RequiredLevel
            )
            {
                updateDtoForValidation.RequiredLevel = updateDto.RequiredLevel;
                hasChanges = true;
            }

            if (
                updateDto.FirstLevelDescription != null
                && updateDto.FirstLevelDescription != existingSkill.FirstLevelDescription
            )
            {
                updateDtoForValidation.FirstLevelDescription = updateDto.FirstLevelDescription;
                hasChanges = true;
            }

            if (
                updateDto.SecondLevelDescription != null
                && updateDto.SecondLevelDescription != existingSkill.SecondLevelDescription
            )
            {
                updateDtoForValidation.SecondLevelDescription = updateDto.SecondLevelDescription;
                hasChanges = true;
            }

            if (
                updateDto.ThirdLevelDescription != null
                && updateDto.ThirdLevelDescription != existingSkill.ThirdLevelDescription
            )
            {
                updateDtoForValidation.ThirdLevelDescription = updateDto.ThirdLevelDescription;
                hasChanges = true;
            }

            if (
                updateDto.FourthLevelDescription != null
                && updateDto.FourthLevelDescription != existingSkill.FourthLevelDescription
            )
            {
                updateDtoForValidation.FourthLevelDescription = updateDto.FourthLevelDescription;
                hasChanges = true;
            }

            // If no changes, return the existing skill
            if (!hasChanges)
            {
                return existingSkill.ToSkillDto();
            }

            // --- Run FluentValidation only on changed fields ---
            var validationResult = await _updateSkillValidator.ValidateAsync(
                updateDtoForValidation
            );
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }

            // --- Additional business logic for changed fields ---

            // Check for duplicate code only if code is being changed
            if (
                !string.IsNullOrWhiteSpace(updateDto.Code)
                && updateDto.Code.Trim() != existingSkill.Code
            )
            {
                var existingSkillWithSameCode = await _skillRepository.GetByCodeAsync(
                    updateDto.Code.Trim()
                );
                if (
                    existingSkillWithSameCode != null
                    && existingSkillWithSameCode.SkillId != skillId
                )
                {
                    throw new ValidationException(
                        $"Skill with code '{updateDto.Code}' already exists."
                    );
                }
                existingSkill.Code = updateDto.Code.Trim();
            }

            // Check if category exists only if category is being changed
            if (
                updateDto.CategoryId.HasValue
                && updateDto.CategoryId.Value != existingSkill.CategoryId
            )
            {
                var category = await _categoryRepository.GetByIdAsync(updateDto.CategoryId.Value);
                if (category == null)
                {
                    throw new ValidationException(
                        $"Category with ID {updateDto.CategoryId} not found."
                    );
                }
                existingSkill.CategoryId = updateDto.CategoryId.Value;
            }

            // Check if subcategory exists only if subcategory is being changed
            if (
                updateDto.SubCategoryId.HasValue
                && updateDto.SubCategoryId.Value != existingSkill.SubCategoryId
            )
            {
                var subCategory = await _categoryRepository.GetSubCategoryByIdAsync(
                    updateDto.SubCategoryId.Value
                );
                if (subCategory == null)
                {
                    throw new ValidationException(
                        $"SubCategory with ID {updateDto.SubCategoryId} not found."
                    );
                }
                existingSkill.SubCategoryId = updateDto.SubCategoryId.Value;
            }

            // --- Apply updates only for changed fields ---
            if (
                !string.IsNullOrWhiteSpace(updateDto.Label)
                && updateDto.Label.Trim() != existingSkill.Label
            )
                existingSkill.Label = updateDto.Label.Trim();

            if (
                !string.IsNullOrWhiteSpace(updateDto.CriticalityLevel)
                && updateDto.CriticalityLevel != existingSkill.CriticalityLevel
            )
                existingSkill.CriticalityLevel = updateDto.CriticalityLevel;

            if (
                updateDto.ProjectRequiresSkill.HasValue
                && updateDto.ProjectRequiresSkill.Value != existingSkill.ProjectRequiresSkill
            )
                existingSkill.ProjectRequiresSkill = updateDto.ProjectRequiresSkill.Value;

            if (
                updateDto.RequiredLevel.HasValue
                && updateDto.RequiredLevel.Value != existingSkill.RequiredLevel
            )
                existingSkill.RequiredLevel = updateDto.RequiredLevel.Value;

            if (
                updateDto.FirstLevelDescription != null
                && updateDto.FirstLevelDescription != existingSkill.FirstLevelDescription
            )
                existingSkill.FirstLevelDescription = updateDto.FirstLevelDescription;

            if (
                updateDto.SecondLevelDescription != null
                && updateDto.SecondLevelDescription != existingSkill.SecondLevelDescription
            )
                existingSkill.SecondLevelDescription = updateDto.SecondLevelDescription;

            if (
                updateDto.ThirdLevelDescription != null
                && updateDto.ThirdLevelDescription != existingSkill.ThirdLevelDescription
            )
                existingSkill.ThirdLevelDescription = updateDto.ThirdLevelDescription;

            if (
                updateDto.FourthLevelDescription != null
                && updateDto.FourthLevelDescription != existingSkill.FourthLevelDescription
            )
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
