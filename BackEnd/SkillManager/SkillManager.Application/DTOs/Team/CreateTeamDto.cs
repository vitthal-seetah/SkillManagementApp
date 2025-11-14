using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillManager.Application.DTOs.Team
{
    public class CreateTeamDto
    {
        public string TeamName { get; set; } = string.Empty;
        public string TeamDescription { get; set; } = string.Empty;
        public int TeamLeadId { get; set; }
        public int ProjectId { get; set; }
    }
}
