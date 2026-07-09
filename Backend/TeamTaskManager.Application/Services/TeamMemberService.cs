using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.API_Respons;
using TeamTaskManager.Application.DTOs.TeamMember;
using TeamTaskManager.Application.Interfaces.Repositories;
using TeamTaskManager.Application.Interfaces.Services;
using TeamTaskManager.Domain.Entities;

namespace TeamTaskManager.Application.Services
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TeamMemberService(IUnitOfWork unitOfWork, IMapper mapper) 
        { _unitOfWork = unitOfWork; _mapper = mapper; }
        public async Task<Response<bool>> AddMember(Guid teamId, Guid userId)
        {
            var team = await _unitOfWork.Teams.GetById(teamId);
            if (team is null)
                return Response<bool>.Failure($"Team with id ({teamId}) was not found.");

            var isAlreadyMember = await _unitOfWork.TeamMembers.IsUserInTeam(teamId, userId);
            if (isAlreadyMember)
                return Response<bool>.Failure("User is already a member of this team.");

            var member = new TeamMember
            {
                TeamId = teamId,
                UserId = userId,
                JoinedAt = DateTime.UtcNow
            };

            await _unitOfWork.TeamMembers.Add(member);
            await _unitOfWork.saveChanges();

            return Response<bool>.Success(true);
        }

        public async Task<Response<IEnumerable<TeamMemberDto>>> GetByTeamId(Guid teamId)
        {
            var team = await _unitOfWork.Teams.GetById(teamId);
            if (team is null)
                return Response<IEnumerable<TeamMemberDto>>.Failure($"Team with id ({teamId}) was not found.");

            var members = await _unitOfWork.TeamMembers.GetByTeamId(teamId);
            return Response<IEnumerable<TeamMemberDto>>.Success(_mapper.Map<IEnumerable<TeamMemberDto>>(members));
        }

        public async Task<Response<bool>> RemoveMember(Guid teamId, Guid userId)
        {
            var member = await _unitOfWork.TeamMembers.GetMember(teamId, userId);
            if (member is null)
                return Response<bool>.Failure("Member not found in this team.");

            await _unitOfWork.TeamMembers.Delete(member);
            await _unitOfWork.saveChanges();

            return Response<bool>.Success(true);
        }
    }
}
