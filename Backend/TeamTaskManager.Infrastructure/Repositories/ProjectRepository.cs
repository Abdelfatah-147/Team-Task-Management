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
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        private readonly AppDbContext _context;
        public ProjectRepository(AppDbContext context) : base(context) { _context = context; }

        public async Task<IEnumerable<Project>> GetByTeamId(Guid teamId, ProjectStatus? status = null)
        {
            var query = _context.Projects.Where(p => p.TeamId == teamId);

            if (status.HasValue)
                query = query.Where(p => p.Status == status);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetByUserId(Guid userId)
        {
            return await _context.Projects
                .Where(p => _context.TeamMembers.Any(tm => tm.UserId == userId && tm.TeamId == p.TeamId))
                .ToListAsync();
        }

    }
}
