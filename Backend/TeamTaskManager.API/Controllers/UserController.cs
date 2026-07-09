using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeamTaskManager.Domain.Entities;

namespace TeamTaskManager.API.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        public UserController(UserManager<AppUser> userManager) { _userManager = userManager; }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> GetAll()
        {
            var users = _userManager.Users.Select(u => new
            {
                u.Id,
                u.FullName,
                u.Email,
                u.IsActive,
                u.CreatedAt
            });

            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
                return NotFound($"User with id ({id}) was not found.");

            return Ok(new
            {
                user.Id,
                user.FullName,
                user.Email,
                user.IsActive,
                user.CreatedAt
            });
        }

        [HttpPut("{id}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRole(Guid id, [FromBody] string newRole)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
                return NotFound($"User with id ({id}) was not found.");

            var validRoles = new[] { "Admin", "Manager", "Member" };
            if (!validRoles.Contains(newRole))
                return BadRequest("Invalid role");

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, newRole);

            return Ok($"Role changed to {newRole} successfully.");
        }

        [HttpPut("{id}/deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
                return NotFound($"User with id ({id}) was not found.");
            
            user.IsActive = false;
            await _userManager.UpdateAsync(user);

            return Ok("User deactivate successfully.");
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var user = await _userManager.FindByIdAsync(GetUserId().ToString());
            if (user is null)
                return NotFound();
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                user.Id,
                user.FullName,
                user.Email,
                user.IsActive,
                user.CreatedAt,
                Roles = roles
            });
        }


    }
}
