using System.ComponentModel.DataAnnotations;

namespace SkillManager.Domain.Entities;

public class UserSkill
{
    [Key]
    public int Id { get; set; }

    // Link to user by Id only (Infrastructure manages the actual ApplicationUser)
    [Required]
    public string UserId { get; set; } = default!;

    // Link to Skill
    [Required]
    public int SkillId { get; set; }

    // Navigation property to Skill
    public Skill Skill { get; set; } = default!;

    // Skill level (1 = Notion, 2 = Pratique, 3 = Maîtrise, 4 = Expert)
    [Range(1, 4)]
    public int Level { get; set; }

    // Optional: track last update
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}
