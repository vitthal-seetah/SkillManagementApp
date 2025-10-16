using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillManager.Domain.Entities;

public class Skill
{
    public int Id { get; set; }

    [Required]
    public string Code { get; set; } = default!; // e.g. "FCT", "PJT", "TEC", etc.

    [Required]
    public string Name { get; set; } = default!; // e.g. "ASP.NET Core", "Produits financiers"

    public string? Description { get; set; }

    public int SkillSectionId { get; set; }

    [ForeignKey(nameof(SkillSectionId))]
    public SkillSection SkillSection { get; set; } = default!;

    public int? Level { get; set; } // optional level mapping

    // ✅ Add this to link to UserSkill
    public ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
}
