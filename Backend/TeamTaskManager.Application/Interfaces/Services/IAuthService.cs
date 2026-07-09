using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.API_Respons;
using TeamTaskManager.Application.DTOs.Auth;

namespace TeamTaskManager.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Response<AuthResponseDto>> RegisterAsync(RegisterDto Rdto);
        Task<Response<AuthResponseDto>> LoginAsync(LoginDto Ldto);
    }
}
