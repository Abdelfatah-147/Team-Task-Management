using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.Interfaces.Repositories;
using TeamTaskManager.Domain.Entities;
using TeamTaskManager.Infrastructure.Data;

namespace TeamTaskManager.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public ITeamRepository Teams { get; }
        public ITeamMemberRepository TeamMembers { get; }
        public IProjectRepository Projects { get; }
        public ITaskRepository Tasks { get; }
        public ICommentRepository Comments { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Teams = new TeamRepository(context);
            TeamMembers = new TeamMemberRepository(context);
            Projects = new ProjectRepository(context);
            Tasks = new TaskRepository(context);
            Comments = new CommentRepository(context);
        }

        public async Task<int> saveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
