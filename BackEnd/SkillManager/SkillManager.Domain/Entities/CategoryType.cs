using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Domain.Entities
{
    public enum CategoryTypeEnum
    {
        Functional = 1,
        Technical = 2,
    }

    public class CategoryType
    {
        [Key]
        public int CategoryTypeId { get; set; }

        [Required]
        public CategoryTypeEnum Name { get; set; }

        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
