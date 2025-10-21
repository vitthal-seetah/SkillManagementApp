using System.ComponentModel.DataAnnotations;

namespace SkillManager.Domain.Entities
{
    public class ApplicationSuite
    {
        [Key]
        public int SuiteId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Perimeter { get; set; } // Functional or business scope

        // Navigation property: one suite can have many applications
        public virtual ICollection<Application> Applications { get; set; } =
            new List<Application>();
    }
}
