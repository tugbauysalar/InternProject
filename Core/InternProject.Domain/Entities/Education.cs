using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternProject.Domain.Entities
{
    public class Education : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; } 
        public Category Category { get; set; }
    }
}
