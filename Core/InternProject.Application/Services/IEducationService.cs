using InternProject.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternProject.Application.Services
{
    public interface IEducationService
    {
        Task<CreateEducationDto> CreateEducation(CreateEducationDto dto);
        Task<UpdateEducationDto> UpdateEducation(int id, UpdateEducationDto dto);
        Task DeleteEducation(int id);
    }
}
