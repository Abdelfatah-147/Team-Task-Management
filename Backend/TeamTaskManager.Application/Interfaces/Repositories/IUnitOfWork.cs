using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Domain.Entities;

namespace TeamTaskManager.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ITeamRepository Teams { get; }
        ITeamMemberRepository TeamMembers { get; }
        IProjectRepository Projects { get; }
        ITaskRepository Tasks { get; }
        ICommentRepository Comments { get; }

        Task<int> saveChanges();

    }
}
