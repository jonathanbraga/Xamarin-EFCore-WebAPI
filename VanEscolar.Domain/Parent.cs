using System;
using System.Collections.Generic;

namespace VanEscolar.Domain
{
    public class Parent
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Neighborhood { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string City { get; set; }
        public string Complement { get; set; }
        public List<Student> Students { get; set; }
        public Link Link { get; set; }
        public List<Message> Messages { get; set; }
    }
}
