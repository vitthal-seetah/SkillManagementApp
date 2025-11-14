using System.ComponentModel.DataAnnotations;

namespace SkillManager.Application.DTOs.Team
{
    public class UpdateTeamDto
    {
        [Required]
        [StringLength(100)]
        public string TeamName { get; set; } = string.Empty;

        [StringLength(500)]
        public string TeamDescription { get; set; } = string.Empty;

        [Required]
        public int TeamLeadId { get; set; }
        public int TeamId { get; set; }
        public int ProjectId { get; set; } // Add this
    }
}
