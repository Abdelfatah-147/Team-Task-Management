using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Domain.Entities;
using TeamTaskManager.Domain.Enums;

namespace TeamTaskManager.Application.Interfaces.Repositories
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<IEnumerable<Project>> GetByTeamId(Guid teamId, ProjectStatus? status = null);
        Task<IEnumerable<Project>> GetByUserId(Guid userId);

    }

}
