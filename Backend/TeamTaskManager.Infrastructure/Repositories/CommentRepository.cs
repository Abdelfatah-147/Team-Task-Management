using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.Interfaces.Repositories;
using TeamTaskManager.Domain.Entities;
using TeamTaskManager.Infrastructure.Data;

namespace TeamTaskManager.Infrastructure.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly AppDbContext _context;
        public CommentRepository(AppDbContext context) : base(context) { _context = context; }

        public async Task<IEnumerable<Comment>> GetByTaskId(Guid taskId)
        {
            return await _context.Comments.Include(c => c.User).Where(c => c.TaskId == taskId).ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetByUserId(Guid userId)
        {
            return await _context.Comments.Where(c => c.UserId == userId).ToListAsync();
        }

    }
}
