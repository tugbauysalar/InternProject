using InternProject.Application.DTOs;
using InternProject.Application.Services;
using InternProject.Domain.Entities;
using InternProject.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace InternProject.API.Controllers
{

    [Route("api/[controller]/[action]")]
    public class TeamController : CustomBaseController
    {
        private readonly ITeamService _teamService;
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public TeamController(ITeamService teamService, AppDbContext context, UserManager<User> userManager)
        {
            _teamService = teamService;
            _context = context;
            _userManager = userManager;
        }

        [HttpPut]
        public async Task<IActionResult> AssignTeamLead(int teamId, string leadId)
        {

            var teamLead = await _context.Users.FindAsync(leadId);
            if (teamLead == null)
            {
                return NotFound("Kullanıcı bulunamadı!");
            }

            var role = await _userManager.GetRolesAsync(teamLead);
            if (!role.Contains("Team Lead"))
            {
                return  BadRequest("Takıma eklemek istediğiniz kişi Team Lead rolüne sahip değil!");
            }
            var result = await _teamService.AssignTeamLeadAsync(teamId, leadId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam(CreateTeamDto dto)
        {
            var team = await _teamService.CreateTeam(dto);
            return Ok(team);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTeam(int id, UpdateTeamDto dto)
        {
            var team = await _teamService.UpdateTeam(id, dto);
            return Ok(team);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            await _teamService.DeleteTeam(id);
            return Ok();
        }
    }
}
