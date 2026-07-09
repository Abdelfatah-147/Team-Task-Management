using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.Interfaces.Repositories;
using TeamTaskManager.Domain.Entities;
using TeamTaskManager.Infrastructure.Data;

namespace TeamTaskManager.Infrastructure.Repositories
{
    public class TeamRepository : GenericRepository<Team>, ITeamRepository
    {
        private readonly AppDbContext _context;
        public TeamRepository(AppDbContext context) : base(context) { _context = context; }

        public async Task<IEnumerable<Team>> GetByUserId(Guid userId)
        {
            return await _context.Teams.Where(t => t.Members.Any(m => m.UserId == userId)).ToListAsync();
        }
    }
}
