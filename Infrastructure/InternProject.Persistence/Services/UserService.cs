using AutoMapper;
using AutoMapper.Configuration.Annotations;
using InternProject.Application.DTOs;
using InternProject.Application.Services;
using InternProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternProject.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<CustomResponseDto<UserDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(createUserDto.Email);
            if (existingUser != null) 
            {
                var error = "Bu e-posta sistemde kayıtlı!";
                return CustomResponseDto<UserDto>.Error(404, error);
            }

            var user = new User
            {
                Email = createUserDto.Email,
                UserName = createUserDto.UserName,
                Password = createUserDto.Password,
                NameSurname = createUserDto.NameSurname
            };

            var result = await _userManager.CreateAsync(user, createUserDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                return CustomResponseDto<UserDto>.Error(400, errors);
            }

            return CustomResponseDto<UserDto>.Success(200, _mapper.Map<UserDto>(user));
        }

        public async Task<CustomResponseDto<NoContentDto>> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) 
            {
                var error = "Kullanıcı bulunamadı!";
                return CustomResponseDto<NoContentDto>.Error(404,error);
            }
            await _userManager.DeleteAsync(user);
            return CustomResponseDto<NoContentDto>.Success(204);
        }

        public async Task<string> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            var user = await _userManager.FindByEmailAsync(updateUserDto.Email);
            if (user == null)
            {
                var error = "Kullanıcı bulunamadı!";
                return error;
            }
            user.NameSurname = updateUserDto.NameSurname;
            user.Password = updateUserDto.Password;
            user.Email = updateUserDto.Email;
            user.UserName = updateUserDto.UserName;
            user.ProfilePhoto = updateUserDto.ProfilePhoto;
            user.Skills = updateUserDto.Skills;

            await _userManager.UpdateAsync(user);
            var message = "Kullanıcı bilgileri başarıyla güncellendi.";
            return message;
        }
    }
}
