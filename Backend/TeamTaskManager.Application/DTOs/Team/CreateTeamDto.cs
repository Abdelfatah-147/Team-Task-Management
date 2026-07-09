using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTaskManager.Application.DTOs.Team
{
    public class CreateTeamDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }    
    }
}
