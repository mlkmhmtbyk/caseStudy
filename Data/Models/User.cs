using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace caseStudy.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; } = false;
    }
}