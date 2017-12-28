using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace VanEscolar.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string Password { get; set; }
        public bool IsAtuhorize { get; set; }
        public Link Link { get; set; }
    }
}
