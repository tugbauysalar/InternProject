using AutoMapper;
using InternProject.Application.DTOs;
using InternProject.Application.Services;
using InternProject.Domain.Entities;
using InternProject.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternProject.API.Controllers
{
    public class CategoryController : CustomBaseController
    {
        private readonly IService<Category> _service;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public CategoryController(IMapper mapper, IService<Category> service, AppDbContext context)
        {
            _mapper = mapper;
            _service = service;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryDto categoryDto)
        {
            await _service.AddAsync(_mapper.Map<Category>(categoryDto));
            _context.SaveChanges();
            return Ok(categoryDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _service.GetByIdAsync(id);
            if(category == null)
            {
                return NotFound("Kategori Bulunamadı!");
            }
            await _service.DeleteAsync(category);
            return Ok("Kategori silindi!");
        }
    }
}
