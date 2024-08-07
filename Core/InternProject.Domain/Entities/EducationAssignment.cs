using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternProject.Domain.Entities
{
    public class EducationAssignment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int EducationId { get; set; }
        public Education Education { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime CompletionDate { get; set; }
    }
}
