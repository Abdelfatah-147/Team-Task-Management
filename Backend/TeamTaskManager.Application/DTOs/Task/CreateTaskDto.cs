using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Domain.Enums;

namespace TeamTaskManager.Application.DTOs.Task
{
    public class CreateTaskDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
