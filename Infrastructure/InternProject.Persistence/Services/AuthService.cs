using InternProject.Application;
using InternProject.Application.DTOs;
using InternProject.Application.Services;
using InternProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternProject.Persistence.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly IService<UserRefreshToken> _userRefreshTokenService;
        private readonly IUnitofWork _unitOfWork;

        public AuthService(UserManager<User> userManager, IService<UserRefreshToken> userRefreshTokenService, 
            ITokenService tokenService, IUnitofWork unitOfWork)
        {
            _userManager = userManager;
            _userRefreshTokenService = userRefreshTokenService;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomResponseDto<TokenDto>> CreateRefreshTokenAsync(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenService.Where(x => x.RefreshToken == refreshToken).FirstOrDefaultAsync();
            if (existRefreshToken == null)
            {
                return CustomResponseDto<TokenDto>.Error(404, "Refresh token bulunamadı!");
            }

            var user = await _userManager.FindByIdAsync(existRefreshToken.UserId);
            if (user == null)
            {
                return CustomResponseDto<TokenDto>.Error(404, "Kullanıcı bulunamadı!");
            }

            var tokenDto = _tokenService.CreateToken(user);

            existRefreshToken.RefreshToken = tokenDto.RefreshToken;
            existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration.ToUniversalTime();

            await _unitOfWork.CommitAsync();

            return CustomResponseDto<TokenDto>.Success(200, tokenDto);
        }

        public async Task<CustomResponseDto<TokenDto>> CreateTokenAsync(UserLoginDto userLoginDto)
        {
            if (userLoginDto == null) throw new ArgumentNullException(nameof(userLoginDto));

            var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
            if (user == null) return CustomResponseDto<TokenDto>.Error(400, "E-posta veya şifre yanlış!");

            if (!await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
            {
                return CustomResponseDto<TokenDto>.Error(400, "E-posta veya şifre yanlış!");
            }

            var token = _tokenService.CreateToken(user);

            var userRefreshToken = await _userRefreshTokenService.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();
            if (userRefreshToken == null)
            {
                await _userRefreshTokenService.AddAsync(new UserRefreshToken
                {
                    UserId = user.Id,
                    RefreshToken = token.RefreshToken,
                    Expiration = token.RefreshTokenExpiration.ToUniversalTime()
                });
            }
            else
            {
                userRefreshToken.RefreshToken = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration.ToUniversalTime();
            }

            await _unitOfWork.CommitAsync();

            return CustomResponseDto<TokenDto>.Success(200, token);
        }
    }
}
