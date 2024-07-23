using InternProject.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternProject.Application.Services
{
    public interface IAuthService
    {
        Task<CustomResponseDto<TokenDto>> CreateTokenAsync(UserLoginDto userLoginDto);
        Task<CustomResponseDto<TokenDto>> CreateRefreshTokenAsync(string refreshToken);
    }
}
