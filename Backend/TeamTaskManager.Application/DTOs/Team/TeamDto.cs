using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTaskManager.Application.DTOs.Team
{
    public class TeamDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
