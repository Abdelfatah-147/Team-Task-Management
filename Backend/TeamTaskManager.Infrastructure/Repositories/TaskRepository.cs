using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.Interfaces.Repositories;
using TeamTaskManager.Domain.Entities;
using TeamTaskManager.Domain.Enums;
using TeamTaskManager.Infrastructure.Data;

namespace TeamTaskManager.Infrastructure.Repositories
{
    public class TaskRepository : GenericRepository<Tasks>, ITaskRepository
    {
        protected readonly AppDbContext _context;
        public TaskRepository(AppDbContext context) : base(context) { _context = context; }

        public async Task<IEnumerable<Tasks>> GetByProjectId(Guid projectId, TasksStatus? status = null, TaskPriority? priority = null, Guid? assignedToUserId = null)
        {
            var query = _context.Tasks.Where(t => t.ProjectId == projectId);

            if (status.HasValue)
                query = query.Where(t => t.Status == status);

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority);

            if (assignedToUserId.HasValue)
                query = query.Where(t => t.AssignedToUserId == assignedToUserId);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Tasks>> GetByUserId(Guid userId, TasksStatus? status = null, TaskPriority? priority = null)
        {
            var query = _context.Tasks.Where(t => t.AssignedToUserId == userId);

            if (status.HasValue)
                query = query.Where(t => t.Status == status);

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Tasks>> GetOverdueTasks(Guid projectId)
        {
            return await _context.Tasks.Where(t => t.ProjectId == projectId && t.DueDate < DateTime.UtcNow && t.Status != TasksStatus.Done).ToListAsync();
        }

    }
}
