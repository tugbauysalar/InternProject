using AutoMapper;
using InternProject.Application.DTOs;
using InternProject.Application.Services;
using InternProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly IEducationService _service;
        private readonly IService<Education> _educationService;
        private readonly IMapper _mapper;

        public EducationController(IEducationService service, IService<Education> educationService, IMapper mapper)
        {
            _service = service;
            _educationService = educationService;
            _mapper = mapper;
        }

        [HttpPost] 
        public async Task<IActionResult> AddEducation(CreateEducationDto dto)
        {
            var education = await _service.CreateEducation(dto);
            return Ok(education);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEducation(int id, UpdateEducationDto dto)
        {
            var education = await _service.UpdateEducation(id, dto);
            return Ok(education);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEducation(int id)
        {
            await _service.DeleteEducation(id);
            return Ok();   
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Education>>> GetDeletedEducations()
        {
            return await _educationService.Where(x => x.IsDeleted).ToListAsync();
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<EducationDto>>> GetAllEducations()
        {
            var educations = await _educationService.Where(x => x.IsDeleted == false).ToListAsync();
            var educationDtos = _mapper.Map<List<EducationDto>>(educations);
            return Ok(educationDtos);
        }

        [HttpGet("user-assignments")]
        public IActionResult GetUserEducationAssignments(string id)
        {
            var assignments = _service.GetUserEducationAssignments(id);
            if (assignments.Count == 0)
            {
                return NotFound("Kullanıcıya ait eğitim bulunamadı!");
            }
                
            return Ok(assignments);
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignEducation(string userId, int educationId, int day)
        {
            var education = await _educationService.GetByIdAsync(educationId);
            if (education == null) { return NotFound("Eğitim bulunamadı!"); }
            await _service.AssignEducationToUser(userId, educationId, day);
            return Ok();
        }

    }
}
