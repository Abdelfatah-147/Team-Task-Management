using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.API_Respons;
using TeamTaskManager.Application.DTOs.Project;
using TeamTaskManager.Application.Interfaces.Repositories;
using TeamTaskManager.Application.Interfaces.Services;
using TeamTaskManager.Domain.Entities;

namespace TeamTaskManager.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<ProjectDto>>> GetAllForUser(Guid userId, bool isAdmin)
        {
            var projects = isAdmin
                ? await _unitOfWork.Projects.GetAll()
                : await _unitOfWork.Projects.GetByUserId(userId);

            return Response<IEnumerable<ProjectDto>>.Success(_mapper.Map<IEnumerable<ProjectDto>>(projects));
        }
        public async Task<Response<IEnumerable<ProjectDto>>> GetAll()
        {
            var projects = await _unitOfWork.Projects.GetAll();
            return Response<IEnumerable<ProjectDto>>.Success(_mapper.Map<IEnumerable<ProjectDto>>(projects));
        }
        public async Task<Response<ProjectDto>> Create(CreateProjectDto CPdto, Guid userId)
        {
            var team = await _unitOfWork.Teams.GetById(CPdto.TeamId);
            if (team is null)
                return Response<ProjectDto>.Failure($"Team with id ({CPdto.TeamId}) was not found.");
            var project = _mapper.Map<Project>(CPdto);
            project.CreatedByUserId = userId;

            await _unitOfWork.Projects.Add(project);
            await _unitOfWork.saveChanges();

            return Response<ProjectDto>.Success(_mapper.Map<ProjectDto>(project));
        }

        public async Task<Response<bool>> Delete(Guid id)
        {
            var project = await _unitOfWork.Projects.GetById(id);
            if (project is null)
                return Response<bool>.Failure($"Project with id ({id}) was not found.");

            await _unitOfWork.Projects.Delete(project);
            await _unitOfWork.saveChanges();

            return Response<bool>.Success(true);
        }

        public async Task<Response<ProjectDto>> GetById(Guid id)
        {
            var project = await _unitOfWork.Projects.GetById(id);
            if (project is null)
                return Response<ProjectDto>.Failure($"Project with id ({id}) was not found.");
            return Response<ProjectDto>.Success(_mapper.Map<ProjectDto>(project));
        }

        public async Task<Response<IEnumerable<ProjectDto>>> GetByTeamId(Guid teamId)
        {
            var projects = await _unitOfWork.Projects.GetAll();
            var teamProjects = projects.Where(p => p.TeamId == teamId);

            return Response<IEnumerable<ProjectDto>>.Success(_mapper.Map<IEnumerable<ProjectDto>>(teamProjects));
        }

        public async Task<Response<ProjectDto>> Update(Guid id, UpdateProjectDto UPdto)
        {
            var project = await _unitOfWork.Projects.GetById(id);
            if (project is null)
                return Response<ProjectDto>.Failure($"Project with id ({id}) was not found.");

            _mapper.Map(UPdto, project);
            await _unitOfWork.Projects.Update(project);
            await _unitOfWork.saveChanges();

            return Response<ProjectDto>.Success(_mapper.Map<ProjectDto>(project));
        }
    }
}
