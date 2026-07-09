using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTaskManager.Application.DTOs.Project
{
    public class CreateProjectDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid TeamId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
