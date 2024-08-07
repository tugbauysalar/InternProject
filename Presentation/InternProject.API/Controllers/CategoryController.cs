using AutoMapper;
using InternProject.Application;
using InternProject.Application.DTOs;
using InternProject.Application.Services;
using InternProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IService<Category> _service;
        private readonly IMapper _mapper;
        private readonly IUnitofWork _unitOfWork;

        public CategoryController(IMapper mapper, IService<Category> service, IUnitofWork unitOfWork)
        {
            _mapper = mapper;
            _service = service;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryDto categoryDto)
        {
            await _service.AddAsync(_mapper.Map<Category>(categoryDto));
            categoryDto.CreatedDate = DateTime.UtcNow;
            await _unitOfWork.CommitAsync();
            return Ok(categoryDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCategoryDto categoryDto)
        {
            var category = await _service.GetByIdAsync(id);
            if(category == null || category.IsDeleted) 
            {
                return(NotFound("Güncellenmek istenen kategori bulunamadı!"));
            }
            category.Name = categoryDto.Name;
            category.UpdatedDate = DateTime.UtcNow;
            await _unitOfWork.CommitAsync();
            return Ok("Kategori başarıyla güncellendi!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _service.GetByIdAsync(id);
            if(category == null || category.IsDeleted)
            {
                return NotFound("Kategori Bulunamadı!");
            }
            category.DeletedDate = DateTime.UtcNow;
            category.IsDeleted = true;
            await _unitOfWork.CommitAsync();
            return Ok("Kategori silindi!");
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<GetCategoryDto>>> GetAllCategories()
        {
            var categories = await _service.Where(x => x.IsDeleted == false).ToListAsync();
            var categoryDtos = _mapper.Map<List<GetCategoryDto>>(categories);
            return Ok(categoryDtos);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetDeletedCategories()
        {
            return await _service.Where(x=>x.IsDeleted).ToListAsync();
        }
    }
}
