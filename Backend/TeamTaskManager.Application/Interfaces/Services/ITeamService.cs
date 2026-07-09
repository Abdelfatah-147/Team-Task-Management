using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.API_Respons;
using TeamTaskManager.Application.DTOs.Team;

namespace TeamTaskManager.Application.Interfaces.Services
{
    public interface ITeamService
    {
        Task<Response<TeamDto>> GetById(Guid id);
        Task<Response<IEnumerable<TeamDto>>> GetAll();
        Task<Response<TeamDto>> Create(CreateTeamDto CTdto, Guid userId);
        Task<Response<TeamDto>> Update(Guid id, UpdateTeamDto UTdto);
        Task<Response<bool>> Delete(Guid id);
    }
}
