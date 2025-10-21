using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SkillManager.Domain.Entities
{
    public class SubCategory
    {
        [Key]
        public int SubCategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        // Foreign key to Category
        [Required]
        public int CategoryId { get; set; }

        // Navigation property
        public Category Category { get; set; }

        // Optional: Navigation to Skills
        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
    }
}
