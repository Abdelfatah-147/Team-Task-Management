using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamTaskManager.Application.DTOs.Team;
using TeamTaskManager.Application.Interfaces.Services;

namespace TeamTaskManager.API.Controllers
{
    [Authorize]
    public class TeamController : BaseController
    {
        private readonly ITeamService _teamService;
        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _teamService.GetAll();
            return Ok(res.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var res = await _teamService.GetById(id);
            if (!res.IsSuccess)
                return NotFound(res.ErrorMessage);

            return Ok(res.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create(CreateTeamDto CTdto)
        {
            var res = await _teamService.Create(CTdto, GetUserId());

            if (!res.IsSuccess)
                return BadRequest(res.ErrorMessage);

            return CreatedAtAction(nameof(GetById), new { id = res.Data!.Id }, res.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update(Guid id, UpdateTeamDto UTdto)
        {
            var res = await _teamService.Update(id, UTdto);
            if (!res.IsSuccess)
                return NotFound(res.ErrorMessage);

            return Ok(res.Data);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var res = await _teamService.Delete(id);
            if (!res.IsSuccess)
                return NotFound(res.ErrorMessage);

            return NoContent();
        }
    }
}
