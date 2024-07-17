using InternProject.Application.DTOs;
using InternProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternProject.Application.Services
{
    public interface IUserService
    {
        Task<CustomResponseDto<UserDto>> CreateUserAsync(CreateUserDto createUserDto);
        Task<string> UpdateUserAsync(UpdateUserDto updateUserDto);
        Task<CustomResponseDto<NoContentDto>> DeleteUserAsync(string id);
    }
}
