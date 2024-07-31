using InternProject.Application.DTOs;
using InternProject.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;

namespace InternProject.API.Controllers
{

    [Route("api/[controller]/[action]")]
    public class TeamController : CustomBaseController
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPut]
        public async Task<IActionResult> AssignTeamLead(int teamId, string leadId)
        {
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
            var team = await _teamService.DeleteTeam(id);
            return Ok(team);
        }
    }
}
