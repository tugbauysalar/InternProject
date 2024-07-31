using InternProject.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class RoleManagementController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public RoleManagementController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        
        [HttpPost]
        public async Task<IActionResult> AssignRolesToUser(string userId, [FromBody] string[] roleIds)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı!" });
            }

            foreach (var roleId in roleIds)
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role == null)
                {
                    return BadRequest(new { message = "Atanmak istenen rol bulunamadı!" });
                }

                var result = await _userManager.AddToRoleAsync(user, role.Name);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
            }

            return Ok(new { message = "Rol başarıyla atandı!" });
        }
    }
}
