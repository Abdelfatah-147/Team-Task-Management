using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Domain.Entities;

namespace TeamTaskManager.Application.Interfaces.Repositories
{
    public interface ITeamRepository : IGenericRepository<Team>
    {
        Task<IEnumerable<Team>> GetByUserId(Guid userId);
    }
}
