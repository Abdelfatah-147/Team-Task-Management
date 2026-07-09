using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTaskManager.Domain.Entities
{
    public class Team : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid CreatedByUserId { get; set; }

        public AppUser CreatedBy { get; set; }
        public ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();
        public ICollection<Project> projects { get; set; } = new List<Project>();
    }
}
