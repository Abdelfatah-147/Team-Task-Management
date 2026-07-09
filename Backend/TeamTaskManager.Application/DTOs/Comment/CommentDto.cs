using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTaskManager.Application.DTOs.Comment
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public string UserFullName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
