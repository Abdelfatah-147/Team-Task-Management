using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTaskManager.Application.DTOs.TeamMember
{
    public class TeamMemberDto
    {
        public Guid TeamId { get; set; }
        public Guid UserId { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
