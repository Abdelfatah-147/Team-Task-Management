using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamTaskManager.Application.DTOs.Project;
using TeamTaskManager.Application.Interfaces.Services;

namespace TeamTaskManager.API.Controllers
{
    [Authorize]
    public class ProjectController : BaseController
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var isAdmin = User.IsInRole("Admin");
            var res = await _projectService.GetAllForUser(GetUserId(), isAdmin);
            return Ok(res.Data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var res = await _projectService.GetById(id);
            if (!res.IsSuccess)
                return NotFound(res.ErrorMessage);

            return Ok(res.Data);
        }

        [HttpGet("team/{teamId}")]
        public async Task<IActionResult> GetByTeamId(Guid teamId)
        {
            var res = await _projectService.GetByTeamId(teamId);
            return Ok(res.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create(CreateProjectDto CPdto)
        {
            var res = await _projectService.Create(CPdto, GetUserId());
            if (!res.IsSuccess)
                return BadRequest(res.ErrorMessage);

            return CreatedAtAction(nameof(GetById), new { id = res.Data!.Id }, res.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update(Guid id, UpdateProjectDto UPdto)
        {
            var res = await _projectService.Update(id, UPdto);

            if (!res.IsSuccess)
                return NotFound(res.ErrorMessage);

            return Ok(res.Data);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var res = await _projectService.Delete(id);

            if (!res.IsSuccess)
                return NotFound(res.ErrorMessage);

            return NoContent();
        }
    }
}
