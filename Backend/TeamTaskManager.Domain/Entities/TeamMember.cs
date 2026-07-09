using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTaskManager.Domain.Entities
{
    public class TeamMember : BaseEntity
    {
        public Guid TeamId { get; set; }
        public Guid UserId { get; set; }
        public DateTime JoinedAt { get; set; }

        public Team Team { get; set; }
        public AppUser User { get; set; }
    }
}
