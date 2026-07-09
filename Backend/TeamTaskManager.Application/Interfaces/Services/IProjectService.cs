using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.API_Respons;
using TeamTaskManager.Application.DTOs.Project;

namespace TeamTaskManager.Application.Interfaces.Services
{
    public interface IProjectService
    {
        Task<Response<IEnumerable<ProjectDto>>> GetAll();
        Task<Response<ProjectDto>> GetById(Guid id);
        Task<Response<IEnumerable<ProjectDto>>> GetByTeamId(Guid teamId);
        Task<Response<ProjectDto>> Create(CreateProjectDto CPdto, Guid userId);
        Task<Response<ProjectDto>> Update(Guid id, UpdateProjectDto UPdto);
        Task<Response<bool>> Delete(Guid id);
        Task<Response<IEnumerable<ProjectDto>>> GetAllForUser(Guid userId, bool isAdmin);
    }
}
