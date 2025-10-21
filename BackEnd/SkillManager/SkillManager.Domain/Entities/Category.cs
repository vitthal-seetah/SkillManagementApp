using System.ComponentModel.DataAnnotations;

/*Category – top-level group (“Fonctionnel”, “Technique”)*/
namespace SkillManager.Domain.Entities;

public class Category
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = default!; // e.g. "Fonctionnel" or "Technique"

    public string? Description { get; set; }

    // Navigation Property
    public ICollection<SkillSection> Sections { get; set; } = new List<SkillSection>();
}
