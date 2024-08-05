using AutoMapper;
using InternProject.Application;
using InternProject.Application.DTOs;
using InternProject.Application.Services;
using InternProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InternProject.Persistence.Services
{
    public class TeamService : ITeamService
    {
        private readonly IService<Team> _service;
        private readonly IUnitofWork _unitofwork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;

        public TeamService(IService<Team> service, IUnitofWork unitofwork, IMapper mapper, UserManager<User> userManager, AppDbContext context)
        {
            _service = service;
            _unitofwork = unitofwork;
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
        }

        public async Task AddUserToTeam(int teamId, string userId)
        {
            var team = await _service.GetByIdAsync(teamId);
            var user = await _userManager.FindByIdAsync(userId);

            if (team.Users == null)
            {
                team.Users = new List<User>();
            }

            if (!team.Users.Any(u => u.Id == userId)) 
            {
                team.Users.Add(user);
                team.UpdatedDate = DateTime.UtcNow;
                _service.Update(team);
                await _unitofwork.CommitAsync();
            }
        }


        public async Task<string> AssignTeamLeadAsync(int teamId, string teamLeadId)
        {
            var team = await _service.GetByIdAsync(teamId);
            if (team == null || team.IsDeleted)
            {
                return "Takım bulunamadı!";
            }

            team.TeamLeadId = teamLeadId;
            team.UpdatedDate = DateTime.UtcNow;
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

        public async Task<List<UserDto>> GetUsersInTeam(int teamId)
        {
            var team = await _context.Teams
                             .Include(t => t.Users)
                             .FirstOrDefaultAsync(t => t.Id == teamId && !t.IsDeleted);

            if (team == null)
            {
                return new List<UserDto>();
            }

            var userDtos = _mapper.Map<List<UserDto>>(team.Users);
            return userDtos;
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
