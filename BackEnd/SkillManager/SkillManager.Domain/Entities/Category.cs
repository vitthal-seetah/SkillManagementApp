using System.ComponentModel.DataAnnotations;

/*Category – top-level group (“Fonctionnel”, “Technique”)*/
namespace SkillManager.Domain.Entities;

public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Required]
    public string Name { get; set; }

    // Foreign key to CategoryType
    [Required]
    public int CategoryTypeId { get; set; }

    // Navigation property
    // Navigation properties
    public virtual CategoryType CategoryType { get; set; }
    public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
}
