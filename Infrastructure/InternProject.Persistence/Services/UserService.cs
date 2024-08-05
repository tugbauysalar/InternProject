using AutoMapper;
using InternProject.Application;
using InternProject.Application.DTOs;
using InternProject.Application.Services;
using InternProject.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternProject.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _manager;
        private readonly AppDbContext _context;
        private readonly IUnitofWork _unitofwork;


        public UserService(UserManager<User> manager, IMapper mapper, AppDbContext context, IUnitofWork unitofwork)
        {
            _manager = manager;
            _mapper = mapper;
            _context = context;
            _unitofwork = unitofwork;
        }

        public async Task AddSkillsToUser(string userId, List<string> skills)
        {
            var user = await _manager.FindByIdAsync(userId);

            if (user.Skills == null)
            {
                user.Skills = new List<string>();
            }

            user.Skills.AddRange(skills);
            _context.Users.Update(user);
            await _unitofwork.CommitAsync();
        }

        public async Task<CustomResponseDto<UserDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var existingUser = await _manager.FindByEmailAsync(createUserDto.Email);
            if (existingUser != null)
            {
                var error = "Bu e-posta kayıtlı!";
                return CustomResponseDto<UserDto>.Error(404, error);
            }

            var user = new User()
            {
                UserName = createUserDto.UserName,
                Email = createUserDto.Email,
                NameSurname = createUserDto.NameSurname,
                Password = createUserDto.Password,

            };
            var result = await _manager.CreateAsync(user, createUserDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                return CustomResponseDto<UserDto>.Error(400, errors);
            }

            return CustomResponseDto<UserDto>.Success(200, _mapper.Map<UserDto>(user));
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _manager.FindByIdAsync(id);
            await _manager.DeleteAsync(user);
        }

        public async Task<UserDto> GetUserAsync(string userId)
        {
            var user = await _manager.FindByIdAsync(userId);
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<CustomResponseDto<UserDto>> UpdateUserAsync(string id, UpdateUserDto userDto)
        {
            var user = await _manager.FindByIdAsync(id);
            if (user == null)
            {
                var error = "Kullanıcı bulunamadı!";
                return CustomResponseDto<UserDto>.Error(404, error);
            }
            user.NameSurname = userDto.NameSurname;
            if (userDto.Password == user.Password)
            {
                return CustomResponseDto<UserDto>.Error(400, "Girdiğiniz şifre eskisiyle aynı!");
            }
            user.Password = userDto.Password;
            user.Email = userDto.Email;
            user.UserName = userDto.UserName;
            user.Skills = userDto.Skills;

            var result = await _manager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                return CustomResponseDto<UserDto>.Error(400, errors);
            }

            return CustomResponseDto<UserDto>.Success(200, _mapper.Map<UserDto>(user));
        }
    }
}
