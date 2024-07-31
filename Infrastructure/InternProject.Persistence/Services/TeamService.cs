using AutoMapper;
using InternProject.Application;
using InternProject.Application.DTOs;
using InternProject.Application.Services;
using InternProject.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternProject.Persistence.Services
{
    public class TeamService : ITeamService
    {
        private readonly IService<Team> _service;
        private readonly IUnitofWork _unitofwork;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public TeamService(IService<Team> service, IUnitofWork unitofwork, IMapper mapper, AppDbContext context, UserManager<User> userManager)
        {
            _service = service;
            _unitofwork = unitofwork;
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
        }

        public async Task<string> AssignTeamLeadAsync(int teamId, string teamLeadId)
        {
            var team = await _service.GetByIdAsync(teamId);
            if (team == null || team.IsDeleted)
            {
                return "Takım bulunamadı!";
            }

            team.TeamLeadId = teamLeadId;
            team.UpdatedDate = DateTime.Now;
            await _unitofwork.CommitAsync();

            return "Takım lideri başarıyla eklendi!";
        }

        public async Task<CreateTeamDto> CreateTeam(CreateTeamDto dto)
        {
            await _service.AddAsync(_mapper.Map<Team>(dto));
            await _unitofwork.CommitAsync();
            return dto;
        }

        public async Task<string> DeleteTeam(int id)
        {
            var team = await _service.GetByIdAsync(id);
            if (team == null || team.IsDeleted)
            {
                return "Silinmek istenen takım bulunamadı!";
            }
            team.DeletedDate = DateTime.UtcNow;
            team.IsDeleted = true;
            await _unitofwork.CommitAsync();
            return "Takım başarıyla silindi!";

        }

        public async Task<UpdateTeamDto> UpdateTeam(int id, UpdateTeamDto dto)
        {
            var team = await _service.GetByIdAsync(id);
            team.Name = dto.Name;
            team.UpdatedDate = DateTime.UtcNow;
            await _unitofwork.CommitAsync();
            return dto;
        }
    }
}
