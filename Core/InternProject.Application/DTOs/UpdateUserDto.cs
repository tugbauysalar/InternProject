using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternProject.Application.DTOs
{
    public class UpdateUserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string NameSurname { get; set; }
        public string ConfirmPassword { get; set; }
        public List<string> Skills { get; set; }
    }
}
