using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Domain.Entities;

namespace TeamTaskManager.Application.Interfaces.Repositories
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetByTaskId(Guid taskId);
        Task<IEnumerable<Comment>> GetByUserId(Guid userId);
    }
}
