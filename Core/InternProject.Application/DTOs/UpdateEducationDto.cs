using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternProject.Application.DTOs
{
    public class UpdateEducationDto
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
