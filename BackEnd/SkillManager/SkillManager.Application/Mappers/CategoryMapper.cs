using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Application.DTOs.Category;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Mappers
{
    public static class CategoryMapper
    {
        // From Entity to DTO
        public static CategoryDto ToDto(this Category category)
        {
            if (category == null)
                return null;

            return new CategoryDto
            {
                Id = category.CategoryId,
                Name = category.Name,
                CategoryTypeId = category.CategoryTypeId,
            };
        }
    }
}
