using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.API_Respons;
using TeamTaskManager.Application.DTOs.Comment;

namespace TeamTaskManager.Application.Interfaces.Services
{
    public interface ICommentService
    {
        Task<Response<CommentDto>> GetById(Guid id);
        Task<Response<IEnumerable<CommentDto>>> GetByTaskId(Guid taskId);
        Task<Response<CommentDto>> Create(CreateCommentDto CCdto, Guid userId);
        Task<Response<bool>> Delete(Guid id);
    }
}
