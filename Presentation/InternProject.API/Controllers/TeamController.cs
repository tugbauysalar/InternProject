using InternProject.Application.DTOs;
using InternProject.Application.Services;
using InternProject.Domain.Entities;
using InternProject.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternProject.API.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IService<Team> _service;

        public TeamController(ITeamService teamService, AppDbContext context, UserManager<User> userManager, IService<Team> service)
        {
            _teamService = teamService;
            _context = context;
            _userManager = userManager;
            _service = service;
        }

        [HttpPut()]
        public async Task<IActionResult> AddUserToTeam(int teamId, string userId)
        {
            var team = await _context.Teams.FindAsync(teamId);
            if (team == null || team.IsDeleted)
            {
                return NotFound("Takım bulunamadı!");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı!");
            }

            await _teamService.AddUserToTeam(teamId, userId);
            return Ok();
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, UpdateTeamDto dto)
        {
            var team = await _teamService.UpdateTeam(id, dto);
            return Ok(team);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            await _teamService.DeleteTeam(id);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsersInTeam(int id)
        {
            var users = await _teamService.GetUsersInTeam(id);
            if (users == null || !users.Any())
            {
                return NotFound("Takım bulunamadı veya takımda kullanıcı yok!");
            }
            return Ok(users);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetDeletedTeams()
        {
            return await _service.Where(x => x.IsDeleted).ToListAsync();
        }
    }
}
