using InternProject.Application.DTOs;
using InternProject.Application.Services;
using InternProject.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly IEducationService _service;
        private readonly AppDbContext _context;

        public EducationController(IEducationService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpPost] 
        public async Task<IActionResult> AddEducation(CreateEducationDto dto)
        {
            var education = await _service.CreateEducation(dto);
            return Ok(education);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEducation(int id, UpdateEducationDto dto)
        {
            var education = await _service.UpdateEducation(id, dto);
            return Ok(education);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEducation(int id)
        {
            var education = await _service.DeleteEducation(id);
            return Ok(education);   
        }
    }
}
