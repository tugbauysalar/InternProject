using InternProject.Application.DTOs;
using InternProject.Application.Services;
using InternProject.Application.Validations;
using InternProject.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;
        public AuthController(IAuthService authService, UserManager<User> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
            var validationResult = new UserLoginDtoValidator().Validate(userLoginDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(new { errors });
            }
            var result = await _authService.CreateTokenAsync(userLoginDto);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authService.CreateRefreshTokenAsync(refreshTokenDto.Token);
            return Ok(result);
        }
    }
}
