using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeamTaskManager.Application.API_Respons;
using TeamTaskManager.Application.DTOs.Auth;
using TeamTaskManager.Application.Interfaces;
using TeamTaskManager.Application.Interfaces.Services;
using TeamTaskManager.Domain.Entities;
using TeamTaskManager.Domain.Exceptions;

namespace TeamTaskManager.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtService _jwt;
        public AuthService(UserManager<AppUser> userManager, IJwtService jwt)
        {
            _userManager = userManager;
            _jwt = jwt;
        }

        public async Task<Response<AuthResponseDto>> RegisterAsync(RegisterDto Rdto)
        {
            var user = new AppUser
            {
                FullName = Rdto.FullName,
                Email = Rdto.Email,
                UserName = Rdto.UserName
            };

            var result = await _userManager.CreateAsync(user, Rdto.Password);

            if (!result.Succeeded)
                return Response<AuthResponseDto>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "Admin");

            var roles = await _userManager.GetRolesAsync(user);

            return Response<AuthResponseDto>.Success(new AuthResponseDto
            {
                Token = _jwt.GenerateToken(user, roles),
                Email = user.Email!,
                FullName = user.FullName
            });
        }

        public async Task<Response<AuthResponseDto>> LoginAsync(LoginDto Ldto)
        {
            var user = await _userManager.FindByEmailAsync(Ldto.Email);

            if (user is null)
                return Response<AuthResponseDto>.Failure("Invalid email or password.");

            if (!await _userManager.CheckPasswordAsync(user, Ldto.Password))
                return Response<AuthResponseDto>.Failure("Invalid email or password.");

            var roles = await _userManager.GetRolesAsync(user);

            return Response<AuthResponseDto>.Success(new AuthResponseDto
            {
                Token = _jwt.GenerateToken(user, roles),
                Email = user.Email!,
                FullName = user.FullName
            });
        }
    }
}
