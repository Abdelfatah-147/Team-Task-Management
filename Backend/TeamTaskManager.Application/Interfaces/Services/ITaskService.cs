using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.API_Respons;
using TeamTaskManager.Application.DTOs.Task;

namespace TeamTaskManager.Application.Interfaces.Services
{
    public interface ITaskService
    {
        Task<Response<TaskDto>> GetById(Guid id);
        Task<Response<IEnumerable<TaskDto>>> GetByProjectId(Guid projectId);
        Task<Response<TaskDto>> Create(CreateTaskDto CTdto, Guid userId);
        Task<Response<TaskDto>> Update(Guid id, UpdateTaskDto UTdto);
        Task<Response<bool>> Delete(Guid id);
    }
}
