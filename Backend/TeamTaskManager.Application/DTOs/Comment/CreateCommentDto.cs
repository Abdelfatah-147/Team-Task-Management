using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTaskManager.Application.DTOs.Comment
{
    public class CreateCommentDto
    {
        public string Content { get; set; }
        public Guid TaskId { get; set; }
    }
}
