using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Application.DTOs.SubCategory;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Mappers
{
    public static class SubCategoryMapper
    {
        public static SubCategory ToSubCategory(SubCategoryDto subCategoryDto)
        {
            return new SubCategory { Name = subCategoryDto.Name };
        }
    }
}
