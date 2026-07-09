using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTaskManager.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }

        public Tasks Task { get; set; }
        public AppUser User { get; set; }
    }
}
