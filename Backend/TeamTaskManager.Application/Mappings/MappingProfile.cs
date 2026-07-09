using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.DTOs.Comment;
using TeamTaskManager.Application.DTOs.Project;
using TeamTaskManager.Application.DTOs.Task;
using TeamTaskManager.Application.DTOs.Team;
using TeamTaskManager.Application.DTOs.TeamMember;
using TeamTaskManager.Domain.Entities;

namespace TeamTaskManager.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Team, TeamDto>();
            CreateMap<CreateTeamDto, Team>();
            CreateMap<UpdateTeamDto, Team>();

            CreateMap<Project, ProjectDto>();
            CreateMap<CreateProjectDto, Project>();
            CreateMap<UpdateProjectDto, Project>();

            CreateMap<Tasks, TaskDto>();
            CreateMap<CreateTaskDto, Tasks>();
            CreateMap<UpdateTaskDto, Tasks>();

            CreateMap<TeamMember, TeamMemberDto>();

            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.UserFullName,
                    opt => opt.MapFrom(src => src.User.FullName));
            CreateMap<CreateCommentDto, Comment>();
        }
    }
}
