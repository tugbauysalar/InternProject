using AutoMapper;
using InternProject.Application;
using InternProject.Application.DTOs;
using InternProject.Application.Services;
using InternProject.Domain.Entities;

namespace InternProject.Persistence.Services
{
    public class TeamService : ITeamService
    {
        private readonly IService<Team> _service;
        private readonly IUnitofWork _unitofwork;
        private readonly IMapper _mapper;

        public TeamService(IService<Team> service, IUnitofWork unitofwork, IMapper mapper)
        {
            _service = service;
            _unitofwork = unitofwork;
            _mapper = mapper;
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

        public async Task DeleteTeam(int id)
        {
            var team = await _service.GetByIdAsync(id);
            team.DeletedDate = DateTime.UtcNow;
            team.IsDeleted = true;
            await _unitofwork.CommitAsync();
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
