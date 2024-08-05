using InternProject.Application.DTOs;
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
        Task<CustomResponseDto<UserDto>> UpdateUserAsync(string id , UpdateUserDto userDto);
        Task DeleteUserAsync(string id);
        Task AddSkillsToUser(string userId, List<string> skills);
        Task<UserDto> GetUserAsync(string userId);
    }
}
