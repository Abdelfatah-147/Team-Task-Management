using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.API_Respons;
using TeamTaskManager.Application.DTOs.Team;
using TeamTaskManager.Application.Interfaces.Repositories;
using TeamTaskManager.Application.Interfaces.Services;
using TeamTaskManager.Domain.Entities;

namespace TeamTaskManager.Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TeamService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<TeamDto>> Create(CreateTeamDto CTdto, Guid userId)
        {
            var team = _mapper.Map<Team>(CTdto);
            team.CreatedByUserId = userId;
            await _unitOfWork.Teams.Add(team);
            await _unitOfWork.saveChanges();

            return Response<TeamDto>.Success(_mapper.Map<TeamDto>(team));
        }

        public async Task<Response<bool>> Delete(Guid id)
        {
            var team = await _unitOfWork.Teams.GetById(id);

            if (team is null)
                return Response<bool>.Failure($"Team with id ({id}) was not found.");

            await _unitOfWork.Teams.Delete(team);
            await _unitOfWork.saveChanges();

            return Response<bool>.Success(true);
        }

        public async Task<Response<IEnumerable<TeamDto>>> GetAll()
        {
            var teams = await _unitOfWork.Teams.GetAll();
            return Response<IEnumerable<TeamDto>>.Success(_mapper.Map<IEnumerable<TeamDto>>(teams));
        }

        public async Task<Response<TeamDto>> GetById(Guid id)
        {
            var team = await _unitOfWork.Teams.GetById(id);
            if (team is null)
                return Response<TeamDto>.Failure($"Team with id ({id}) was not found.");
            return Response<TeamDto>.Success(_mapper.Map<TeamDto>(team));
        }

        public async Task<Response<TeamDto>> Update(Guid id, UpdateTeamDto UTdto)
        {
            var team = await _unitOfWork.Teams.GetById(id);
            if (team is null)
                return Response<TeamDto>.Failure($"Team with id ({id}) was not found.");
            _mapper.Map(UTdto, team);
            await _unitOfWork.Teams.Update(team);
            await _unitOfWork.saveChanges();

            return Response<TeamDto>.Success(_mapper.Map<TeamDto>(team));
        }
    }
}
