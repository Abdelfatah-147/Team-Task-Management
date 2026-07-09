using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using TeamTaskManager.Application.Interfaces;
using TeamTaskManager.Application.Interfaces.Services;
using TeamTaskManager.Application.Mappings;
using TeamTaskManager.Application.Services;
using TeamTaskManager.Application.Validators.Auth;

namespace TeamTaskManager.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(op => op.AddProfile<MappingProfile>());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<ITeamMemberService, TeamMemberService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IJwtService, JwtService>();

            return services;
        } 
    }
}
