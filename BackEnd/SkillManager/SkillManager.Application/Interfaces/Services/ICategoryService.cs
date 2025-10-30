using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Application.Models;

namespace SkillManager.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<CategoryNavigationViewModel> GetCategoryNavigationAsync(
            int? selectedCategoryId = null
        );
        Task<List<CategoryTypeWithCategories>> GetCategoryTypesWithCategoriesAsync();
    }
}
