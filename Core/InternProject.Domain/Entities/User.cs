
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternProject.Domain.Entities
{
    public class User : IdentityUser
    {
        public string NameSurname { get; set; }
        public string Password { get; set; }
        public List<string> ? Skills { get; set; }
        public Team Team { get; set; }
        public int ? TeamId { get; set; }

    }
}
