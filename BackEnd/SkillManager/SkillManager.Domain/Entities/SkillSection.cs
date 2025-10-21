using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/*SkillSection – middle grouping (“CONNAISSANCES METIER”, “GESTION DE PROJET”, etc.)*/

namespace SkillManager.Domain.Entities;

public class SkillSection
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = default!; // e.g. "CONNAISSANCES METIER", "GESTION DE PROJET", etc.

    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; } = default!;

    // Navigation property
    public ICollection<Skill> Skills { get; set; } = new List<Skill>();
}
