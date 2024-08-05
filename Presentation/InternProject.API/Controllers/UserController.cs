using InternProject.Application.DTOs;
using InternProject.Application.Services;
using InternProject.Application.Validations;
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

        [HttpPut]
        public async Task<IActionResult> UpdateUser(string id, UpdateUserDto userDto)
        {
            var validationResult = new UpdateUserDtoValidator().Validate(userDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(new { errors });
            }
            return CreateIActionResult(await _userService.UpdateUserAsync(id, userDto));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok();
        }

        [HttpPut("addskills")]
        public async Task<IActionResult> AddSkillsToUser(string userId, List<string> skills)
        {
            await _userService.AddSkillsToUser(userId, skills);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserAsync(id);
            return Ok(user);
        }
    }
}
