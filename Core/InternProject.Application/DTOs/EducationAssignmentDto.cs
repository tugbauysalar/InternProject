using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternProject.Application.DTOs
{
    public class EducationAssignmentDto
    {
        public string EducationName { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime CompletionDate { get; set; }
    }
}
