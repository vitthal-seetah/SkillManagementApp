namespace SkillManager.Application.DTOs.Team
{
    public class TeamDto
    {
        public int TeamId { get; set; }

        public string TeamName { get; set; } = string.Empty;

        public string TeamDescription { get; set; } = string.Empty;

        public int TeamLeadId { get; set; }

        public int MemberCount { get; set; }
    }
}
