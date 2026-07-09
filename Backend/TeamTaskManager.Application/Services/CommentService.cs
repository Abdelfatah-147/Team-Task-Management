using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.API_Respons;
using TeamTaskManager.Application.DTOs.Comment;
using TeamTaskManager.Application.Interfaces.Repositories;
using TeamTaskManager.Application.Interfaces.Services;
using TeamTaskManager.Domain.Entities;

namespace TeamTaskManager.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<CommentDto>> GetById(Guid id)
        {
            var comment = await _unitOfWork.Comments.GetById(id);
            if (comment is null)
                return Response<CommentDto>.Failure($"Comment with id ({id}) was not found.");

            return Response<CommentDto>.Success(_mapper.Map<CommentDto>(comment));
        }

        public async Task<Response<CommentDto>> Create(CreateCommentDto CCdto, Guid userId)
        {
            var task = await _unitOfWork.Tasks.GetById(CCdto.TaskId);
            if (task is null)
                return Response<CommentDto>.Failure($"Task with id ({CCdto.TaskId}) was not found.");
            var comment = _mapper.Map<Comment>(CCdto);
            comment.UserId = userId;

            await _unitOfWork.Comments.Add(comment);
            await _unitOfWork.saveChanges();

            return Response<CommentDto>.Success(_mapper.Map<CommentDto>(comment));
        }

        public async Task<Response<bool>> Delete(Guid id)
        {
            var comment = await _unitOfWork.Comments.GetById(id);
            if (comment is null)
                return Response<bool>.Failure($"Comment with id ({id}) was not found.");

            await _unitOfWork.Comments.Delete(comment);
            await _unitOfWork.saveChanges();

            return Response<bool>.Success(true);
        }

        public async Task<Response<IEnumerable<CommentDto>>> GetByTaskId(Guid taskId)
        {
            var task = await _unitOfWork.Tasks.GetById(taskId);
            if (task is null)
                return Response<IEnumerable<CommentDto>>.Failure($"Task with Id ({taskId}) was not found.");
            var comments = await _unitOfWork.Comments.GetAll();
            var taskComments = comments.Where(c => c.TaskId == taskId);

            return Response<IEnumerable<CommentDto>>.Success(_mapper.Map<IEnumerable<CommentDto>>(taskComments));
        }
    }
}
