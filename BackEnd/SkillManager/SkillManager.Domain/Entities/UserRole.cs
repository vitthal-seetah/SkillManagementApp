using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SkillManager.Domain.Entities
{
    public class UserRole
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        // Navigation properties
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
