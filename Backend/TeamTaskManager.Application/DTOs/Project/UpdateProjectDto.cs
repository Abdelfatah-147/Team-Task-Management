using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Domain.Enums;

namespace TeamTaskManager.Application.DTOs.Project
{
    public class UpdateProjectDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ProjectStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
