using System;
using System.Collections.Generic;
using System.Text;

namespace VanEscolar.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Authorize { get; set; }
        public Link Link { get; set; }
    }
}
