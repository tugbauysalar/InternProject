using InternProject.Application.DTOs;
using InternProject.Application.Services;
using InternProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InternProject.Persistence.Services
{
    public class TokenService : ITokenService
    {
        private readonly CustomTokenOption _customTokenOption;
        private readonly UserManager<User> _userManager;

        public TokenService(IOptions<CustomTokenOption> options, UserManager<User> userManager)
        {
            _customTokenOption = options.Value;
            _userManager = userManager;
        }

        private string CreateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(User user)
        {
            var userList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                userList.Add(new Claim(ClaimTypes.Role, role));
            }

            return userList;
        }

        public async Task<TokenDto> CreateTokenAsync(User user)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.RefreshTokenExpiration);
            var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOption.SecurityKey);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = await GetClaimsAsync(user);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _customTokenOption.Issuer,
                audience: _customTokenOption.Audience,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: claims,
                signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration
            };

            return tokenDto;
        }
    }
}
