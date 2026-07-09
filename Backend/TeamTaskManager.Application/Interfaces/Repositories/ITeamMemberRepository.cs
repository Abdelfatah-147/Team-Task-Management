using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Domain.Entities;

namespace TeamTaskManager.Application.Interfaces.Repositories
{
    public interface ITeamMemberRepository : IGenericRepository<TeamMember>
    {
        Task<IEnumerable<TeamMember>> GetByTeamId(Guid teamId);
        Task<bool> IsUserInTeam(Guid teamId, Guid userId);
        Task<TeamMember?> GetMember(Guid teamId, Guid userId);
    }
}
