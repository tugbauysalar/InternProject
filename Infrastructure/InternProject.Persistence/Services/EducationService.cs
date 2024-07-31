using AutoMapper;
using InternProject.Application;
using InternProject.Application.DTOs;
using InternProject.Application.Services;
using InternProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternProject.Persistence.Services
{
    public class EducationService : IEducationService
    {
        private readonly IService<Education> _service;
        private readonly IMapper _mapper;
        private readonly IUnitofWork _unitofwork;

        public EducationService(IService<Education> service, IMapper mapper, IUnitofWork unitofWork)
        {
            _service = service;
            _mapper = mapper;
            _unitofwork = unitofWork;
        }

        public async Task<CreateEducationDto> CreateEducation(CreateEducationDto dto)
        {
            await _service.AddAsync(_mapper.Map<Education>(dto));
            await _unitofwork.CommitAsync();
            return dto; 
        }

        public async Task<string> DeleteEducation(int id)
        {
            var education = await _service.GetByIdAsync(id);
            if ( education == null || education.IsDeleted )
            {
                return "Silinmek istenen eğitim bulunamadı!";
            }
            education.DeletedDate = DateTime.Now;
            education.IsDeleted = true;
            await _unitofwork.CommitAsync();
            return "Eğitim başarıyla silindi!";
        }

        public async Task<UpdateEducationDto> UpdateEducation(int id, UpdateEducationDto dto)
        {
            var education = await _service.GetByIdAsync(id);
            education.Name = dto.Name;
            education.CategoryId = dto.CategoryId;
            education.UpdatedDate = DateTime.Now;
            await _unitofwork.CommitAsync();
            return dto;
        }
    }
}
