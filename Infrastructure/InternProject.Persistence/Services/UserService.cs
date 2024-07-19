using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly UserManager<User> _manager;

        public UserService(UserManager<User> manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
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
    }
}
