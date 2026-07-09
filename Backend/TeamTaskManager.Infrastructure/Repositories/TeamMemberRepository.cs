using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.Interfaces.Repositories;
using TeamTaskManager.Domain.Entities;
using TeamTaskManager.Infrastructure.Data;

namespace TeamTaskManager.Infrastructure.Repositories
{
    public class TeamMemberRepository : GenericRepository<TeamMember>, ITeamMemberRepository
    {
        private readonly AppDbContext _context;
        public TeamMemberRepository(AppDbContext context) : base(context) { _context = context; }

        public async Task<IEnumerable<TeamMember>> GetByTeamId(Guid teamId)
        {
            return await _context.TeamMembers
                .Where(m => m.TeamId == teamId).ToListAsync();
        }

        public async Task<bool> IsUserInTeam(Guid teamId, Guid userId)
        {
            return await _context.TeamMembers.AnyAsync(m => m.TeamId == teamId && m.UserId == userId);
        }
        public async Task<TeamMember?> GetMember(Guid teamId, Guid userId)
        {
            return await _context.TeamMembers.FirstOrDefaultAsync(m => m.TeamId == teamId && m.UserId == userId);
        }
    }
}
