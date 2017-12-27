using System;
using System.Collections.Generic;
using System.Text;

namespace VanEscolar.Domain
{
    public class Link
    {
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
        public Parent Parent { get; set; }
    }
}
