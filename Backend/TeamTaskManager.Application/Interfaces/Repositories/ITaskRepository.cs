using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Domain.Entities;
using TeamTaskManager.Domain.Enums;

namespace TeamTaskManager.Application.Interfaces.Repositories
{
    public interface ITaskRepository : IGenericRepository<Tasks>
    {
        Task<IEnumerable<Tasks>> GetByProjectId(Guid projectId, TasksStatus? status = null, TaskPriority? priority = null, Guid? assignedToUserId = null);
        Task<IEnumerable<Tasks>> GetByUserId(Guid userId, TasksStatus? status = null, TaskPriority? priority = null);
        Task<IEnumerable<Tasks>> GetOverdueTasks(Guid projectId);
    }
}
