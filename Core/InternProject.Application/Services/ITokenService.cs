using InternProject.Application.DTOs;
using InternProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternProject.Application.Services
{
    public interface ITokenService
    {
        TokenDto CreateToken(User user); 
    }
}
