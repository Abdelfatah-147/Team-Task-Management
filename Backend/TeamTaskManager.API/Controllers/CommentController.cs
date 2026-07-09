using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamTaskManager.Application.DTOs.Comment;
using TeamTaskManager.Application.Interfaces.Services;

namespace TeamTaskManager.API.Controllers
{
    [Authorize]
    public class CommentController : BaseController
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetByTaskId(Guid taskId)
        {
            var res = await _commentService.GetByTaskId(taskId);
            if (!res.IsSuccess)
                return NotFound(res.ErrorMessage);

            return Ok(res.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentDto CCdto)
        {
            var res = await _commentService.Create(CCdto, GetUserId());
            if (!res.IsSuccess)
                return BadRequest(res.ErrorMessage);

            return Ok(res.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var comment = await _commentService.GetById(id);

            if (!comment.IsSuccess)
                return NotFound(comment.ErrorMessage);

            var isAdminOrManager = User.IsInRole("Admin") || User.IsInRole("Manager");
            var isOwner = comment.Data!.UserId == GetUserId();

            if (!isAdminOrManager && !isOwner)
                return Forbid();

            var result = await _commentService.Delete(id);

            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return NoContent();
        }

    }
}
