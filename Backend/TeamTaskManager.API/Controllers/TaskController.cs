using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamTaskManager.Application.DTOs.Task;
using TeamTaskManager.Application.Interfaces.Services;

namespace TeamTaskManager.API.Controllers
{
    [Authorize]
    public class TaskController : BaseController
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var res = await _taskService.GetById(id);
            if (!res.IsSuccess)
                return NotFound(res.ErrorMessage);

            return Ok(res.Data);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetByProjectId(Guid projectId)
        {
            var res = await _taskService.GetByProjectId(projectId);
            return Ok(res.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create(CreateTaskDto CTdto)
        {
            var res = await _taskService.Create(CTdto, GetUserId());
            if (!res.IsSuccess)
                return BadRequest(res.ErrorMessage);

            return CreatedAtAction(nameof(GetById), new { id = res.Data!.Id }, res.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update(Guid id, UpdateTaskDto UTdto)
        {
            var res = await _taskService.Update(id, UTdto);
            if (!res.IsSuccess)
                return NotFound(res.ErrorMessage);

            return Ok(res.Data);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var res = await _taskService.Delete(id);
            if (!res.IsSuccess)
                return NotFound(res.ErrorMessage);

            return NoContent();
        }

    }
}
