using FluentValidation;
using SkillManager.Application.DTOs.Category;
using SkillManager.Application.DTOs.SubCategory;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Application.Mappers;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.Exceptions;

namespace SkillManager.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IValidator<CreateCategoryDto> _createCategoryValidator;
        private readonly IValidator<UpdateCategoryDto> _updateCategoryValidator;
        private readonly IValidator<CreateSubCategoryDto> _createSubCategoryValidator;
        private readonly IValidator<UpdateSubCategoryDto> _updateSubCategoryValidator;
        private readonly IValidator<CreateCategoryTypeDto> _createCategoryTypeValidator;
        private readonly IValidator<UpdateCategoryTypeDto> _updateCategoryTypeValidator;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IValidator<CreateCategoryDto> createCategoryValidator = null,
            IValidator<UpdateCategoryDto> updateCategoryValidator = null,
            IValidator<CreateSubCategoryDto> createSubCategoryValidator = null,
            IValidator<UpdateSubCategoryDto> updateSubCategoryValidator = null,
            IValidator<CreateCategoryTypeDto> createCategoryTypeValidator = null,
            IValidator<UpdateCategoryTypeDto> updateCategoryTypeValidator = null
        )
        {
            _categoryRepository = categoryRepository;
            _createCategoryValidator = createCategoryValidator;
            _updateCategoryValidator = updateCategoryValidator;
            _createSubCategoryValidator = createSubCategoryValidator;
            _updateSubCategoryValidator = updateSubCategoryValidator;
            _createCategoryTypeValidator = createCategoryTypeValidator;
            _updateCategoryTypeValidator = updateCategoryTypeValidator;
        }

        // Category Methods
        public async Task<CategoryDto> GetCategoryByIdAsync(int categoryId)
        {
            if (categoryId <= 0)
                throw new ValidationException("Invalid category ID.");

            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
                throw new NotFoundException($"Category with ID {categoryId} not found.");

            return category.ToCategoryDto();
        }

        public async Task<CategoryDto> GetCategoryByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Category name cannot be empty.");

            var category = await _categoryRepository.GetByNameAsync(name);
            if (category == null)
                throw new NotFoundException($"Category with name '{name}' not found.");

            return category.ToCategoryDto();
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => c.ToCategoryDto());
        }

        public async Task<IEnumerable<SubCategoryDto>> GetAllSubCategoriesAsync()
        {
            var subCategories = await _categoryRepository.GetAllSubCategoriesAsync();
            return subCategories.Select(c => c.ToSubCategoryDto());
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesByTypeAsync(int categoryTypeId)
        {
            if (categoryTypeId <= 0)
                throw new ValidationException("Invalid category type ID.");

            var categoryType = await _categoryRepository.GetCategoryTypeByIdAsync(categoryTypeId);
            if (categoryType == null)
                throw new NotFoundException($"Category type with ID {categoryTypeId} not found.");

            var categories = await _categoryRepository.GetByCategoryTypeAsync(categoryType);
            return categories.Select(c => c.ToCategoryDto());
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto)
        {
            // Validate input
            if (_createCategoryValidator != null)
            {
                var validationResult = await _createCategoryValidator.ValidateAsync(createDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(
                        "; ",
                        validationResult.Errors.Select(e => e.ErrorMessage)
                    );
                    throw new ValidationException(errors);
                }
            }

            // Check if category already exists
            var existingCategory = await _categoryRepository.GetByNameAsync(createDto.Name);
            if (existingCategory != null)
                throw new ValidationException(
                    $"Category with name '{createDto.Name}' already exists."
                );

            // Check if category type exists
            var categoryType = await _categoryRepository.GetCategoryTypeByIdAsync(
                createDto.CategoryTypeId
            );
            if (categoryType == null)
                throw new ValidationException(
                    $"Category type with ID {createDto.CategoryTypeId} not found."
                );

            var category = createDto.ToCategory();

            var created = await _categoryRepository.AddAsync(category);
            if (!created)
                throw new ApplicationException("Failed to create category.");

            // Return the created category
            var newCategory = await _categoryRepository.GetByNameAsync(createDto.Name);
            return newCategory?.ToCategoryDto()
                ?? throw new ApplicationException("Failed to retrieve created category.");
        }

        public async Task<CategoryDto> UpdateCategoryAsync(
            int categoryId,
            UpdateCategoryDto updateDto
        )
        {
            if (categoryId <= 0)
                throw new ValidationException("Invalid category ID.");

            var existingCategory = await _categoryRepository.GetByIdAsync(categoryId);
            if (existingCategory == null)
                throw new NotFoundException($"Category with ID {categoryId} not found.");

            // Validate input
            if (_updateCategoryValidator != null)
            {
                var validationResult = await _updateCategoryValidator.ValidateAsync(updateDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(
                        "; ",
                        validationResult.Errors.Select(e => e.ErrorMessage)
                    );
                    throw new ValidationException(errors);
                }
            }

            // Apply updates
            if (!string.IsNullOrWhiteSpace(updateDto.Name))
            {
                // Check if new name already exists (excluding current category)
                var categoryWithSameName = await _categoryRepository.GetByNameAsync(
                    updateDto.Name.Trim()
                );
                if (categoryWithSameName != null && categoryWithSameName.CategoryId != categoryId)
                    throw new ValidationException(
                        $"Category with name '{updateDto.Name}' already exists."
                    );

                existingCategory.Name = updateDto.Name.Trim();
            }

            if (updateDto.CategoryTypeId.HasValue)
            {
                var categoryType = await _categoryRepository.GetCategoryTypeByIdAsync(
                    updateDto.CategoryTypeId.Value
                );
                if (categoryType == null)
                    throw new ValidationException(
                        $"Category type with ID {updateDto.CategoryTypeId} not found."
                    );

                existingCategory.CategoryTypeId = updateDto.CategoryTypeId.Value;
            }

            var updated = await _categoryRepository.UpdateAsync(existingCategory);
            if (!updated)
                throw new ApplicationException("Failed to update category.");

            var updatedCategory = await _categoryRepository.GetByIdAsync(categoryId);
            return updatedCategory?.ToCategoryDto()
                ?? throw new ApplicationException("Failed to retrieve updated category.");
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            if (categoryId <= 0)
                throw new ValidationException("Invalid category ID.");

            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
                throw new NotFoundException($"Category with ID {categoryId} not found.");

            // Check if category has skills
            if (category.Skills != null && category.Skills.Any())
                throw new ValidationException("Cannot delete category that has associated skills.");

            return await _categoryRepository.DeleteAsync(category);
        }

        // Category Type Methods
        public async Task<CategoryTypeDto> GetCategoryTypeByIdAsync(int categoryTypeId)
        {
            if (categoryTypeId <= 0)
                throw new ValidationException("Invalid category type ID.");

            var categoryType = await _categoryRepository.GetCategoryTypeByIdAsync(categoryTypeId);
            if (categoryType == null)
                throw new NotFoundException($"Category type with ID {categoryTypeId} not found.");

            return categoryType.ToCategoryTypeDto();
        }

        public async Task<CategoryTypeDto> GetCategoryTypeByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Category type name cannot be empty.");

            var categoryType = await _categoryRepository.GetCategoryTypeByNameAsync(name);
            if (categoryType == null)
                throw new NotFoundException($"Category type with name '{name}' not found.");

            return categoryType.ToCategoryTypeDto();
        }

        public async Task<IEnumerable<CategoryTypeDto>> GetAllCategoryTypesAsync()
        {
            var categoryTypes = await _categoryRepository.GetAllCategoryTypesAsync();
            return categoryTypes.Select(ct => ct.ToCategoryTypeDto());
        }

        public async Task<CategoryTypeDto> CreateCategoryTypeAsync(CreateCategoryTypeDto createDto)
        {
            // Validate input
            if (_createCategoryTypeValidator != null)
            {
                var validationResult = await _createCategoryTypeValidator.ValidateAsync(createDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(
                        "; ",
                        validationResult.Errors.Select(e => e.ErrorMessage)
                    );
                    throw new ValidationException(errors);
                }
            }

            // Check if category type already exists
            var existingCategoryType = await _categoryRepository.GetCategoryTypeByNameAsync(
                createDto.Name
            );
            if (existingCategoryType != null)
                throw new ValidationException(
                    $"Category type with name '{createDto.Name}' already exists."
                );

            var categoryType = createDto.ToCategoryType();

            var created = await _categoryRepository.AddCategoryTypeAsync(categoryType);
            if (!created)
                throw new ApplicationException("Failed to create category type.");

            // Return the created category type
            var newCategoryType = await _categoryRepository.GetCategoryTypeByNameAsync(
                createDto.Name
            );
            return newCategoryType?.ToCategoryTypeDto()
                ?? throw new ApplicationException("Failed to retrieve created category type.");
        }

        public async Task<CategoryTypeDto> UpdateCategoryTypeAsync(
            int categoryTypeId,
            UpdateCategoryTypeDto updateDto
        )
        {
            if (categoryTypeId <= 0)
                throw new ValidationException("Invalid category type ID.");

            var existingCategoryType = await _categoryRepository.GetCategoryTypeByIdAsync(
                categoryTypeId
            );
            if (existingCategoryType == null)
                throw new NotFoundException($"Category type with ID {categoryTypeId} not found.");

            // Validate input
            if (_updateCategoryTypeValidator != null)
            {
                var validationResult = await _updateCategoryTypeValidator.ValidateAsync(updateDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(
                        "; ",
                        validationResult.Errors.Select(e => e.ErrorMessage)
                    );
                    throw new ValidationException(errors);
                }
            }

            // Apply updates
            if (!string.IsNullOrWhiteSpace(updateDto.Name))
            {
                // Check if new name already exists (excluding current category type)
                var categoryTypeWithSameName = await _categoryRepository.GetCategoryTypeByNameAsync(
                    updateDto.Name.Trim()
                );
                if (
                    categoryTypeWithSameName != null
                    && categoryTypeWithSameName.CategoryTypeId != categoryTypeId
                )
                    throw new ValidationException(
                        $"Category type with name '{updateDto.Name}' already exists."
                    );

                existingCategoryType.Name = updateDto.Name.Trim();
            }

            var updated = await _categoryRepository.UpdateCategoryTypeAsync(existingCategoryType);
            if (!updated)
                throw new ApplicationException("Failed to update category type.");

            var updatedCategoryType = await _categoryRepository.GetCategoryTypeByIdAsync(
                categoryTypeId
            );
            return updatedCategoryType?.ToCategoryTypeDto()
                ?? throw new ApplicationException("Failed to retrieve updated category type.");
        }

        public async Task<bool> DeleteCategoryTypeAsync(int categoryTypeId)
        {
            if (categoryTypeId <= 0)
                throw new ValidationException("Invalid category type ID.");

            var categoryType = await _categoryRepository.GetCategoryTypeByIdAsync(categoryTypeId);
            if (categoryType == null)
                throw new NotFoundException($"Category type with ID {categoryTypeId} not found.");

            // Check if category type has categories
            var categories = await _categoryRepository.GetByCategoryTypeAsync(categoryType);
            if (categories != null && categories.Any())
                throw new ValidationException(
                    "Cannot delete category type that has associated categories."
                );

            return await _categoryRepository.DeleteCategoryTypeAsync(categoryTypeId);
        }

        // SubCategory Methods
        public async Task<SubCategoryDto> GetSubCategoryByIdAsync(int subCategoryId)
        {
            if (subCategoryId <= 0)
                throw new ValidationException("Invalid subcategory ID.");

            var subCategory = await _categoryRepository.GetSubCategoryByIdAsync(subCategoryId);
            if (subCategory == null)
                throw new NotFoundException($"SubCategory with ID {subCategoryId} not found.");

            return subCategory.ToSubCategoryDto();
        }

        public async Task<IEnumerable<SubCategoryDto>> GetSubCategoriesByCategoryAsync(
            int categoryId
        )
        {
            if (categoryId <= 0)
                throw new ValidationException("Invalid category ID.");

            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
                throw new NotFoundException($"Category with ID {categoryId} not found.");

            var subCategories = await _categoryRepository.GetSubCategoriesByCategoryIdAsync(
                categoryId
            );
            return subCategories.Select(sc => sc.ToSubCategoryDto());
        }

        public async Task<SubCategoryDto> CreateSubCategoryAsync(CreateSubCategoryDto createDto)
        {
            // Validate input
            if (_createSubCategoryValidator != null)
            {
                var validationResult = await _createSubCategoryValidator.ValidateAsync(createDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(
                        "; ",
                        validationResult.Errors.Select(e => e.ErrorMessage)
                    );
                    throw new ValidationException(errors);
                }
            }

            // Check if parent category exists
            var category = await _categoryRepository.GetByIdAsync(createDto.CategoryId);
            if (category == null)
                throw new ValidationException(
                    $"Category with ID {createDto.CategoryId} not found."
                );

            // Check if subcategory with same name already exists in this category
            var exists = await _categoryRepository.SubCategoryExistsAsync(
                createDto.Name.Trim(),
                createDto.CategoryId
            );
            if (exists)
                throw new ValidationException(
                    $"SubCategory with name '{createDto.Name}' already exists in this category."
                );

            var subCategory = createDto.ToSubCategory();

            var created = await _categoryRepository.AddSubCategoryAsync(subCategory);
            if (!created)
                throw new ApplicationException("Failed to create subcategory.");

            // Return the created subcategory
            var newSubCategory = await _categoryRepository.GetSubCategoryByIdAsync(
                subCategory.SubCategoryId
            );
            return newSubCategory?.ToSubCategoryDto()
                ?? throw new ApplicationException("Failed to retrieve created subcategory.");
        }

        public async Task<SubCategoryDto> UpdateSubCategoryAsync(
            int subCategoryId,
            UpdateSubCategoryDto updateDto
        )
        {
            if (subCategoryId <= 0)
                throw new ValidationException("Invalid subcategory ID.");

            var existingSubCategory = await _categoryRepository.GetSubCategoryByIdAsync(
                subCategoryId
            );
            if (existingSubCategory == null)
                throw new NotFoundException($"SubCategory with ID {subCategoryId} not found.");

            // Validate input
            if (_updateSubCategoryValidator != null)
            {
                var validationResult = await _updateSubCategoryValidator.ValidateAsync(updateDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(
                        "; ",
                        validationResult.Errors.Select(e => e.ErrorMessage)
                    );
                    throw new ValidationException(errors);
                }
            }

            // Apply updates
            bool nameChanged = false;
            bool categoryChanged = false;

            if (!string.IsNullOrWhiteSpace(updateDto.Name))
            {
                nameChanged = true;
                existingSubCategory.Name = updateDto.Name.Trim();
            }

            if (updateDto.CategoryId.HasValue)
            {
                categoryChanged = true;
                // Check if new category exists
                var newCategory = await _categoryRepository.GetByIdAsync(
                    updateDto.CategoryId.Value
                );
                if (newCategory == null)
                    throw new ValidationException(
                        $"Category with ID {updateDto.CategoryId} not found."
                    );

                existingSubCategory.CategoryId = updateDto.CategoryId.Value;
            }

            // Check for duplicates if name or category changed
            if (nameChanged || categoryChanged)
            {
                var targetCategoryId = categoryChanged
                    ? updateDto.CategoryId.Value
                    : existingSubCategory.CategoryId;
                var targetName = nameChanged ? updateDto.Name.Trim() : existingSubCategory.Name;

                var exists = await _categoryRepository.SubCategoryExistsAsync(
                    targetName,
                    targetCategoryId
                );
                if (exists)
                {
                    // But allow if it's the same subcategory (updating other fields)
                    var duplicate = await _categoryRepository.GetSubCategoryByIdAsync(
                        subCategoryId
                    );
                    if (duplicate == null || duplicate.SubCategoryId != subCategoryId)
                    {
                        throw new ValidationException(
                            $"SubCategory with name '{targetName}' already exists in this category."
                        );
                    }
                }
            }

            var updated = await _categoryRepository.UpdateSubCategoryAsync(existingSubCategory);
            if (!updated)
                throw new ApplicationException("Failed to update subcategory.");

            var updatedSubCategory = await _categoryRepository.GetSubCategoryByIdAsync(
                subCategoryId
            );
            return updatedSubCategory?.ToSubCategoryDto()
                ?? throw new ApplicationException("Failed to retrieve updated subcategory.");
        }

        public async Task<bool> DeleteSubCategoryAsync(int subCategoryId)
        {
            if (subCategoryId <= 0)
                throw new ValidationException("Invalid subcategory ID.");

            var subCategory = await _categoryRepository.GetSubCategoryByIdAsync(subCategoryId);
            if (subCategory == null)
                throw new NotFoundException($"SubCategory with ID {subCategoryId} not found.");

            // Check if subcategory has skills
            if (subCategory.Skills != null && subCategory.Skills.Any())
                throw new ValidationException(
                    "Cannot delete subcategory that has associated skills."
                );

            return await _categoryRepository.DeleteSubCategoryAsync(subCategoryId);
        }

        // Validation Methods
        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await _categoryRepository.ExistsAsync(categoryId);
        }

        public async Task<bool> CategoryExistsAsync(string categoryName)
        {
            return await _categoryRepository.ExistsAsync(categoryName);
        }

        public async Task<bool> CategoryTypeExistsAsync(int categoryTypeId)
        {
            var categoryType = await _categoryRepository.GetCategoryTypeByIdAsync(categoryTypeId);
            return categoryType != null;
        }

        public async Task<bool> CategoryTypeExistsAsync(string categoryTypeName)
        {
            var categoryType = await _categoryRepository.GetCategoryTypeByNameAsync(
                categoryTypeName
            );
            return categoryType != null;
        }

        public async Task<bool> SubCategoryExistsAsync(int subCategoryId)
        {
            var subCategory = await _categoryRepository.GetSubCategoryByIdAsync(subCategoryId);
            return subCategory != null;
        }

        public async Task<bool> SubCategoryExistsAsync(string subCategoryName, int categoryId)
        {
            return await _categoryRepository.SubCategoryExistsAsync(subCategoryName, categoryId);
        }
    }
}
