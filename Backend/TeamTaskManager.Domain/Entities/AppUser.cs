using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTaskManager.Domain.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<TeamMember> TeamMembers { get; set; }
        public ICollection<Tasks> AssignedTasks { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}
