namespace SkillManager.Domain.Entities
{
    public class Team
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;

        public string TeamDescription { get; set; } = string.Empty;

        public int? TeamLeadId { get; set; }

        // Navigation properties
        public virtual User? TeamLead { get; set; } = null!;
        public virtual ICollection<User> Members { get; set; } = new List<User>();
        public virtual ICollection<ProjectTeam> ProjectTeams { get; set; } =
            new List<ProjectTeam>();
    }
}
