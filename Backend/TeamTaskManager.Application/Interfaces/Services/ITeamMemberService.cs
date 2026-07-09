using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.API_Respons;
using TeamTaskManager.Application.DTOs.TeamMember;
using TeamTaskManager.Domain.Entities;

namespace TeamTaskManager.Application.Interfaces.Services
{
    public interface ITeamMemberService
    {
        Task<Response<IEnumerable<TeamMemberDto>>> GetByTeamId(Guid teamId);
        Task<Response<bool>> AddMember(Guid teamId, Guid userId);
        Task<Response<bool>> RemoveMember(Guid teamId, Guid userId);
    }
}
