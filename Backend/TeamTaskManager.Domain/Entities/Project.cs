using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Domain.Enums;

namespace TeamTaskManager.Domain.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid TeamId { get; set; }
        public Guid CreatedByUserId { get; set; }
        public ProjectStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Team Team { get; set; }
        public AppUser CreatedBy { get; set; }
        public ICollection<Tasks> Tasks { get; set; }
    }
}
