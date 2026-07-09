using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Domain.Enums;

namespace TeamTaskManager.Domain.Entities
{
    public class Tasks : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid ProjectId { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public TasksStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }


        public Project Project { get; set; }
        public AppUser CreatedBy { get; set; }
        public AppUser? AssignedTo { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
