using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternProject.Domain.Entities
{
    public class Team : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? TeamLeadId { get; set; }
        public User? TeamLead { get; set; }
        public List<User>? Users { get; set; }
    }
}
