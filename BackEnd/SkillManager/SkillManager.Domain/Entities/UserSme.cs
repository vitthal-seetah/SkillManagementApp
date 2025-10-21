namespace SkillManager.Domain.Entities
{
    public class UserSME
    {
        public string UserId { get; set; }
        public int SkillId { get; set; }
        public int CategoryTypeId { get; set; }

        // Navigation properties
        public virtual Skill Skill { get; set; }
        public virtual CategoryType CategoryType { get; set; }
    }
}
