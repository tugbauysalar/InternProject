using AutoMapper;
using InternProject.Application.DTOs;
using InternProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternProject.Application.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile() 
        {
            CreateMap<User, UserDto>();
            CreateMap<User, CreateUserDto>();
            CreateMap<User, UserLoginDto>();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, GetCategoryDto>().ReverseMap();
            CreateMap<Team, CreateTeamDto>().ReverseMap();
            CreateMap<Team, UpdateTeamDto>().ReverseMap();
            CreateMap<Education, CreateEducationDto>().ReverseMap();
            CreateMap<Education,  UpdateEducationDto>().ReverseMap();
            CreateMap<Education, EducationDto>().ReverseMap();
        }
    }
}
