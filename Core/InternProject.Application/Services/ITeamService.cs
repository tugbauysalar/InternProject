using InternProject.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternProject.Application.Services
{
    public interface ITeamService
    {
        Task<CreateTeamDto> CreateTeam(CreateTeamDto dto);
        Task<UpdateTeamDto> UpdateTeam(int id, UpdateTeamDto dto);
        Task DeleteTeam(int id);
        Task<string> AssignTeamLeadAsync(int teamId, string teamLeadId);
    }
}
