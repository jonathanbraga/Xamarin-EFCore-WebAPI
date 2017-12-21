using System.Collections.Generic;

namespace VanEscolar.Domain
{
    public class Parent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Neighborhood { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string City { get; set; }
        public string Complement { get; set; }
        public string Email { get; set; }
        public List<Student> Students { get; set; }
    }
}
