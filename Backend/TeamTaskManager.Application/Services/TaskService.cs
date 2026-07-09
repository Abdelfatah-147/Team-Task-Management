using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.API_Respons;
using TeamTaskManager.Application.DTOs.Task;
using TeamTaskManager.Application.Interfaces.Repositories;
using TeamTaskManager.Application.Interfaces.Services;
using TeamTaskManager.Domain.Entities;
using TeamTaskManager.Domain.Enums;

namespace TeamTaskManager.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public TaskService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<Response<TaskDto>> Create(CreateTaskDto CTdto, Guid userId)
        {
            var project = await _unitOfWork.Projects.GetById(CTdto.ProjectId);
            if (project is null)
                return Response<TaskDto>.Failure($"Project with id ({CTdto.ProjectId}) was not found.");
            if (CTdto.AssignedToUserId.HasValue)
            {
                var assihnedUser = await _userManager.FindByIdAsync(CTdto.AssignedToUserId.Value.ToString());
                if (assihnedUser is null)
                    return Response<TaskDto>.Failure($"User with id ({CTdto.AssignedToUserId}) was not found.");
            }
            var task = _mapper.Map<Tasks>(CTdto);
            task.CreatedByUserId = userId;
            task.Status = TasksStatus.Todo;

            await _unitOfWork.Tasks.Add(task);
            await _unitOfWork.saveChanges();

            return Response<TaskDto>.Success(_mapper.Map<TaskDto>(task));
        }

        public async Task<Response<bool>> Delete(Guid id)
        {
            var task = await _unitOfWork.Tasks.GetById(id);
            if (task is null)
                return Response<bool>.Failure($"Task with id ({id}) was not found.");

            await _unitOfWork.Tasks.Delete(task);
            await _unitOfWork.saveChanges();

            return Response<bool>.Success(true);
        }

        public async Task<Response<TaskDto>> GetById(Guid id)
        {
            var task = await _unitOfWork.Tasks.GetById(id);
            if (task is null)
                return Response<TaskDto>.Failure($"Task with id ({id}) was not found.");
            return Response<TaskDto>.Success(_mapper.Map<TaskDto>(task));
        }

        public async Task<Response<IEnumerable<TaskDto>>> GetByProjectId(Guid projectId)
        {
            var tasks = await _unitOfWork.Tasks.GetAll();
            var projectTasks = tasks.Where(t => t.ProjectId == projectId);
            return Response<IEnumerable<TaskDto>>.Success(_mapper.Map<IEnumerable<TaskDto>>(projectTasks));
        }

        public async Task<Response<TaskDto>> Update(Guid id, UpdateTaskDto UTdto)
        {
            var task = await _unitOfWork.Tasks.GetById(id);
            if (task is null)
                return Response<TaskDto>.Failure($"Task with id ({id}) was not found.");
            _mapper.Map(UTdto, task);
            await _unitOfWork.Tasks.Update(task);
            await _unitOfWork.saveChanges();

            return Response<TaskDto>.Success(_mapper.Map<TaskDto>(task));
        }
    }
}
