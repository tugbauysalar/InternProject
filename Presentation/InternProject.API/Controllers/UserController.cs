using InternProject.Application.DTOs;
using InternProject.Application.Services;
using InternProject.Application.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternProject.API.Controllers
{
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            var validationResult = new CreateUserDtoValidator().Validate(createUserDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(new { errors });
            }
            return CreateIActionResult(await _userService.CreateUserAsync(createUserDto));
        }
    }
}
