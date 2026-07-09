using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamTaskManager.Application.Interfaces.Repositories;
using TeamTaskManager.Application.Interfaces.Services;
using TeamTaskManager.Domain.Entities;

namespace TeamTaskManager.API.Controllers
{
    public class TeamMemberController : BaseController
    {
        private readonly ITeamMemberService _teamMemberService;
        public TeamMemberController(ITeamMemberService teamMemberService) { _teamMemberService = teamMemberService; }

        [HttpGet("{teamId}")]
        public async Task<IActionResult> GetByTeamId(Guid teamId)
        {
            var result = await _teamMemberService.GetByTeamId(teamId);
            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> AddMember(Guid teamId, Guid userId)
        {
            var result = await _teamMemberService.AddMember(teamId, userId);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> RemoveMember(Guid teamId, Guid userId)
        {
            var result = await _teamMemberService.RemoveMember(teamId, userId);
            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return NoContent();
        }
    }
}
