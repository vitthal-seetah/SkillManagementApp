using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Application.DTOs.Category;

public class UpdateCategoryDto
{
    [StringLength(100)]
    public string? Name { get; set; }

    public int? CategoryTypeId { get; set; }
}
